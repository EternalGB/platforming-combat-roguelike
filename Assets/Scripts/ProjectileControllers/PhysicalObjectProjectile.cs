using UnityEngine;
using System.Collections;

public class PhysicalObjectProjectile : PoolableProjectile
{
	
	public float velocityThreshold;

	protected override void OnCollisionEnter2D(Collision2D col)
	{
		if(col.relativeVelocity.magnitude >= velocityThreshold)
			CollisionHandler(col.collider);
	}


}

