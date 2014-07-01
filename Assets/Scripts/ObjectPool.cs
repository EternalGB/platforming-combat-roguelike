using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{

	public const string poolPrefabLocation = "Assets/Prefabs/objectPool.prefab";
	public static Dictionary<string,ObjectPool> currentPools;
	public GameObject pooledObject;
	public int pooledAmount = 20;
	public bool growable = true;

	List<GameObject> pool;

	void Awake()
	{
		if(currentPools == null)
			currentPools = new Dictionary<string, ObjectPool>();
		currentPools.Add(gameObject.name,this);
	}

	public static ObjectPool GetPoolByName(string name)
	{
		return currentPools[name];
	}

	public static ObjectPool GetPoolByRepresentative(GameObject rep)
	{	
		if(currentPools == null)
			currentPools = new Dictionary<string, ObjectPool>();
		ObjectPool pool = null;
		if(!currentPools.TryGetValue(rep.gameObject.name + "Pool", out pool)) {

			pool = ((GameObject)GameObject.Instantiate
			        (Resources.LoadAssetAtPath<GameObject>(poolPrefabLocation)))
				.GetComponent<ObjectPool>();
			pool.init(rep,10,true);
			pool.gameObject.name = rep.gameObject.name + "Pool";
			currentPools.Add(pool.gameObject.name, pool);
		}
		return pool;
	}


	void Start()
	{
		if(pooledObject != null)
			init(pooledObject, pooledAmount, growable);
	}

	void init(GameObject pooledObject, int pooledAmount, bool growable) 
	{
		pool = new List<GameObject>();
		this.pooledObject = pooledObject;
		this.pooledAmount = pooledAmount;
		this.growable = growable;
		for(int i = 0; i < pooledAmount; i++) {
			GameObject obj = (GameObject)Instantiate(pooledObject);
			obj.SetActive(false);
			pool.Add(obj);
		}
	}

	public GameObject getPooled()
	{
		for(int i = 0; i < pool.Count; i++) {
			if(!pool[i].activeInHierarchy) {
				return pool[i];
			}
		}

		if(growable) {
			GameObject obj = (GameObject)Instantiate(pooledObject);
			pool.Add(obj);
			return obj;
		}

		return null;
	}

}

