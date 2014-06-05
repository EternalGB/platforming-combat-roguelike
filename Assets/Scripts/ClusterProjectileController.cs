using UnityEngine;
using System.Collections;

public class ClusterProjectileController : PoolableProjectile
{

	public GameObject clusterObj;
	ObjectPool clusterPool;
	public int clusterSize;
	public float clusterForce;

	void Start()
	{
		if(clusterObj != null) {
			clusterPool = ObjectPool.GetPoolByRepresentative(clusterObj);
		}
	}

	void OnDisable()
	{
		CancelInvoke();
		if(clusterPool != null) {
			for(int i = 0; i < clusterSize; i++) {
				GameObject proj = clusterPool.getPooled();
				proj.SetActive(true);
				proj.transform.position = transform.position;
				proj.rigidbody2D.AddForce(Random.insideUnitCircle*clusterForce);
			}
		}
	}

	void SetClusterObject(GameObject obj)
	{
		clusterObj = obj;
	}

}

