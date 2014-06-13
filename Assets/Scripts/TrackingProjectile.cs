using UnityEngine;
using System.Collections;


public class TrackingProjectile : PoolableProjectile
{
	public Transform target;
	public bool tracking = true;
	public float speed;
	public float rearmingTime = 1f;
	public float maxTurn = 20;
	public float instability = 20;
	Vector3 destination;

	void FixedUpdate()
	{
		if(target != null) {
			destination = target.position;
		} else {
			if(!IsInvoking("findTarget"))
				Invoke("findTarget",rearmingTime);
			//otherwise just go in the direction we're already going
			destination = transform.position + transform.right;
		}

		//jiggle the destination a bit
		float instabAngle = Random.Range (-instability,instability);
		Vector3 toDir = (destination - transform.position).normalized;
		toDir = Quaternion.AngleAxis(instabAngle,Vector3.forward)*toDir;

		//face the destination
		transform.right = Vector3.RotateTowards(transform.right,toDir,maxTurn*Time.fixedDeltaTime,1);

		//appy force
		rigidbody2D.velocity = transform.right*speed;

	}

	void EnableTracking()
	{
		tracking = true;
	}

	void findTarget()
	{
		//TODO may have to swap this to some kind of culled list eventually if it gets too slow
		GameObject nearest = ActiveEnemiesSingleton.Instance.GetClosestEnemy(transform.position);
		if(nearest == null)
			target = null;
		else
			target = nearest.transform;
	}
}

