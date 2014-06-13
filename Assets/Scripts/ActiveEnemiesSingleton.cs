using UnityEngine;
using System.Collections.Generic;

public class ActiveEnemiesSingleton : MonoBehaviour
{

	private Dictionary<int, GameObject> enemies;
	public static ActiveEnemiesSingleton Instance
	{
		get; private set;
	}

	void Awake()
	{
		if(Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}

		enemies = new Dictionary<int, GameObject>();

		Instance = this;
	}

	public void AddEnemy(GameObject enemy)
	{
		enemies.Add(enemy.GetInstanceID(),enemy);
	}

	public bool RemoveEnemy(GameObject enemy)
	{
		return enemies.Remove(enemy.GetInstanceID());
	}
	
	public GameObject GetClosestEnemy(Vector2 worldPos)
	{
		float bestDist = float.MaxValue;
		GameObject best = null;
		foreach(GameObject enemy in enemies.Values) {
			float dist = Vector2.Distance(worldPos,enemy.transform.position);
			if(dist < bestDist) {
				best = enemy;
				bestDist = dist;
			}
		}
		return best;
	}

	public GameObject getFirstWithinRadius(Vector2 worldPos, float radius)
	{
		foreach(GameObject enemy in enemies.Values) {
			float dist = Vector2.Distance(worldPos,enemy.transform.position);
			if(dist < radius) {
				return enemy;
			}
		}
		return null;
	}




}

