using UnityEngine;
using System.Collections;
using System;



public class ProjectileBlocker : MonoBehaviour
{

	public int numCanBlock;
	public int numBlocked = 0;
	public LayerMask absorbFrom;

	public delegate void ProjBlockerDestroyedEvent(ProjectileBlocker blocker);
	public event ProjBlockerDestroyedEvent destroyEvent;

	void OnCollisionEnter2D(Collision2D col)
	{
		PoolableProjectile proj;
		if(proj = col.gameObject.GetComponent<PoolableProjectile>()) {
			print("Shield collided with " + gameObject.name);
			if(numBlocked < numCanBlock && (absorbFrom.value &1 << col.gameObject.layer) != 0) {
				print("Shield Destroying " + gameObject.name);
				proj.Destroy();
				numBlocked++;
			}
		}

		if(numBlocked == numCanBlock)
			Destroy(gameObject);
	}

	void OnDestroy()
	{
		destroyEvent(this);
	}


		
}

