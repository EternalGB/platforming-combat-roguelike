using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{



	public GameObject pooledObject;
	public int pooledAmount = 20;
	public bool growable = true;

	List<GameObject> pool;






	void Start()
	{
		if(pooledObject != null)
			init(pooledObject, pooledAmount, growable);
	}

	public void init(GameObject pooledObject, int pooledAmount, bool growable) 
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

