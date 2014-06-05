using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{

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
		return currentPools[rep.name + "Pool"];
	}

	void Start()
	{
		pool = new List<GameObject>();
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

