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

	
	protected void Start()
	{
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
	
	
}

