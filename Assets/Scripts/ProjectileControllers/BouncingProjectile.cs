using UnityEngine;
using System.Collections.Generic;

public class BouncingProjectile : PoolableProjectile
{

	public Transform target;
	public int numBounces;
	public float detectionRadius;
	public float speed;
	public float maxTurn = 360;
	Vector3 destination;
	int bounceCount = 0;
	Collider2D[] nearbyEnemies;
	int enemyListSize = 50;
	List<Transform> lastTargets;

	void FixedUpdate()
	{
		if(target != null) {
			destination = target.position;
		} else {
			//otherwise just go in the direction we're already going
			destination = transform.position + transform.right;
		}

		Vector3 toDir = (destination - transform.position).normalized;
		
		//face the destination
		transform.right = Vector3.RotateTowards(transform.right,toDir,maxTurn*Time.fixedDeltaTime,1);
		
		//appy force
		rigidbody2D.velocity = transform.right*speed;
	}

	override protected void OnDisable()
	{
		target = null;
		if(lastTargets != null)
			lastTargets.Clear();
		bounceCount = 0;
		base.OnDisable();
	}

	protected override void OnTriggerEnter2D(Collider2D col)
	{
		if(lastTargets == null)
			lastTargets = new List<Transform>();
		if(onCollision != null && (onCollisionTargets.value &1 << col.gameObject.layer) != 0) {
			if(bounceCount < numBounces) {
				if(target) {
					if(target.GetInstanceID() == col.transform.GetInstanceID()) {
						CollisionHandler(col);
						lastTargets.Add(target);
						target = getNextTarget(lastTargets);
					}
				} else {
					CollisionHandler(col);
					lastTargets.Add(col.transform);
					target = getNextTarget(lastTargets);
					if(target == null)
						Destroy();
				}
				bounceCount++;
				if(bounceCount == numBounces)
					Destroy();
			}
		}
	}

	Transform getNextTarget(List<Transform> lastTargets)
	{
		if(nearbyEnemies == null)
			nearbyEnemies = new Collider2D[enemyListSize];
		Physics2D.OverlapCircleNonAlloc(transform.position,detectionRadius,nearbyEnemies,onCollisionTargets.value);
		float bestDist = float.MaxValue;
		Transform best = null;
		float dist = 0;
		foreach(Collider2D col in nearbyEnemies) {
			if(col != null && !lastTargets.Contains(col.transform)) {
				dist = Vector2.Distance(transform.position,col.transform.position);
				if(dist < bestDist) {
					best = col.transform;
					bestDist = dist;
				}
			}
		}
		return best;
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position,detectionRadius);
	}


}

