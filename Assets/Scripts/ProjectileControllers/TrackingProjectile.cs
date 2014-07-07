using UnityEngine;
using System.Collections;


public class TrackingProjectile : PoolableProjectile
{
	public Transform target;
	public bool tracking = true;
	public float speed;
	public float rearmingTime = 1f;
	public float detectionRadius;
	public float maxTurn = 20;
	public float instability = 20;
	Vector3 destination;
	Collider2D[] nearbyEnemies;
	int enemyListSize = 50;

	void FixedUpdate()
	{
		float actualInstab = instability;
		if(target != null) {
			destination = target.position;
		} else {
			actualInstab *= 2;
			if(!IsInvoking("findTarget"))
				Invoke("findTarget",rearmingTime);
			//otherwise just go in the direction we're already going
			destination = transform.position + transform.right;
		}

		//jiggle the destination a bit
		float instabAngle = Random.Range (-actualInstab,actualInstab);
		Vector3 toDir = (destination - transform.position).normalized;
		if(instability > 0)
			toDir = Quaternion.AngleAxis(instabAngle,Vector3.forward)*toDir;

		//face the destination
		transform.right = Vector3.RotateTowards(transform.right,toDir,maxTurn*Time.fixedDeltaTime,1);

		//appy force
		rigidbody2D.velocity = transform.right*speed;

	}

	override protected void Reset()
	{
		target = null;
		base.Reset();
	}

	void EnableTracking()
	{
		tracking = true;
	}

	void findTarget()
	{
		if(nearbyEnemies == null)
			nearbyEnemies = new Collider2D[enemyListSize];
		Physics2D.OverlapCircleNonAlloc(transform.position,detectionRadius,nearbyEnemies,onCollisionTargets.value);
		float bestDist = float.MaxValue;
		Transform best = null;
		float dist = 0;
		foreach(Collider2D col in nearbyEnemies) {
			if(col != null) {
				dist = Vector2.Distance(transform.position,col.transform.position);
				if(dist < bestDist) {
					best = col.transform;
					bestDist = dist;
				}
			}
		}
		target = best;
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position,detectionRadius);
	}
}

