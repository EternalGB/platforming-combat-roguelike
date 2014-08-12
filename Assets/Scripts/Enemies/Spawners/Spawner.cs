using UnityEngine;
using System.Collections.Generic;

public abstract class Spawner : MonoBehaviour
{
	
	public enum SpawnType
	{
		CIRCLE,HORI_LINE,VERT_LINE,SQUARE
	}
	
	public GameObject enemyType;
	public SpawnType spawnType;
	public float range;
	public float spawnInterval;
	public bool randomisedIntervals;
	float interval;
	
	protected void Start()
	{
		SpawnCaller();
	}

	protected void SpawnCaller()
	{
		if(randomisedIntervals)
			interval = Random.Range (0,spawnInterval);
		else
			interval = spawnInterval;

		if(spawnType == SpawnType.CIRCLE) {
			Invoke("SpawnCircle",interval);
		} else if(spawnType == SpawnType.HORI_LINE) {
			Invoke("SpawnLineHorizontal",interval);
		} else if(spawnType == SpawnType.VERT_LINE) {
			Invoke("SpawnLineVertical",interval);
		} else if(spawnType == SpawnType.SQUARE) {
			Invoke("SpawnSquare",interval);
		}

		Invoke("SpawnCaller",interval);
	}

	protected void SpawnCircle()
	{
		Spawn((Vector2)transform.position + Random.insideUnitCircle*range);
	}
	
	protected void SpawnLineHorizontal()
	{
		Spawn(new Vector2(transform.position.x + Random.Range (-range,range),transform.position.y));
	}

	protected void SpawnLineVertical()
	{
		Spawn(new Vector2(transform.position.x,transform.position.y + Random.Range (-range,range)));
	}
	
	protected void SpawnSquare()
	{
		float halfRange = range/2;
		Spawn(new Vector2(transform.position.x + Random.Range(-halfRange,halfRange),
		                  transform.position.y + Random.Range(-halfRange,halfRange)));
	}

	protected abstract void Spawn(Vector2 pos);

	protected GameObject Create(Vector2 pos)
	{
		if(!Physics2D.OverlapPoint(pos)) {
			GameObject enemy = (GameObject)GameObject.Instantiate(enemyType,pos,Quaternion.identity);
			FlyingGuard flying;
			if(flying = enemy.GetComponent<FlyingGuard>())
				flying.SetTether(transform,range);
			return enemy;
		} else
			return null;
	}



	void OnDrawGizmosSelected()
	{
		Vector3 pos = transform.position;
		if(spawnType == SpawnType.CIRCLE) {
			Gizmos.DrawWireSphere(pos,range);
		} else if(spawnType == SpawnType.HORI_LINE) {
			Gizmos.DrawLine(pos - Vector3.right*range,pos + Vector3.right*range);
		} else if(spawnType == SpawnType.SQUARE) {
			Gizmos.DrawWireCube(pos,new Vector3(range,range,1));
		} else if(spawnType == SpawnType.VERT_LINE) {
			Gizmos.DrawLine(pos - Vector3.up*range,pos + Vector3.up*range);
		}
	}

}

