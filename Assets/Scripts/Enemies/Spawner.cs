using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{

	public enum SpawnType
	{
		CIRCLE,HORI_LINE,VERT_LINE,SQUARE
	}

	public GameObject enemyType;
	public SpawnType spawnType;
	public float range;
	public float spawnInterval;
	public float capacity;
	public List<GameObject> spawned;

	void Start()
	{
		spawned = new List<GameObject>();
		if(spawnType == SpawnType.CIRCLE) {
			InvokeRepeating("SpawnCircle",spawnInterval,spawnInterval);
		} else if(spawnType == SpawnType.HORI_LINE) {
			InvokeRepeating("SpawnLineHorizontal",spawnInterval,spawnInterval);
		} else if(spawnType == SpawnType.VERT_LINE) {
			InvokeRepeating("SpawnLineVertical",spawnInterval,spawnInterval);
		} else if(spawnType == SpawnType.SQUARE) {
			InvokeRepeating("SpawnSquare",spawnInterval,spawnInterval);
		}
	}

	void SpawnCircle()
	{
		Spawn((Vector2)transform.position + Random.insideUnitCircle*range);
	}

	void SpawnLineHorizontal()
	{
		Spawn(new Vector2(transform.position.x + Random.Range (-range,range),transform.position.y));
	}

	void SpawnLineVertical()
	{
		Spawn(new Vector2(transform.position.x,transform.position.y + Random.Range (-range,range)));
	}

	void SpawnSquare()
	{
		float halfRange = range/2;
		Spawn(new Vector2(transform.position.x + Random.Range(-halfRange,halfRange),
		                  transform.position.y + Random.Range(-halfRange,halfRange)));
	}

	void Spawn(Vector2 pos)
	{
		//clean up the list
		for(int i = 0; i < spawned.Count; i++) {
			if(spawned[i] == null) {
				spawned.RemoveAt(i);
				i--;
			}
		}
		if(spawned.Count < capacity) {
			GameObject enemy = (GameObject)GameObject.Instantiate(enemyType,pos,Quaternion.identity);
			if(enemy.GetComponent<FlyingGuard>())
				enemy.SendMessage("SetTether",transform);
			AddChild(enemy);
		}
	}

	void AddChild(GameObject child)
	{
		spawned.Add(child);
	}



}

