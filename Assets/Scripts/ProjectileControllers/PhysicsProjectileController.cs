using UnityEngine;
using System.Collections;

public class PhysicsProjectileController : PoolableProjectile
{
	

	void FixedUpdate()
	{
		transform.up = rigidbody2D.velocity.normalized;
		//transform.right = Vector3.Slerp(transform.right, rigidbody2D.velocity.normalized, Time.deltaTime);
	}
	
}

