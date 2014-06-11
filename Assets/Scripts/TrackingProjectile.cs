using UnityEngine;
using System.Collections;


public class TrackingProjectile : PoolableProjectile
{
	public Transform target;
	public bool tracking = true;
	public float speed;
	public float rearmingTime = 1f;
	public float stability = 5;

	void FixedUpdate()
	{
		if(target != null) {
			rigidbody2D.velocity = (target.position - transform.position).normalized*speed;
		} else if(tracking) {
			target = findTarget();
			if(target == null) {
				tracking = false;
				Invoke("EnableTracking",rearmingTime);
			}
		} else {
			//move randomly
			if(rigidbody2D.velocity.magnitude > 0) {
				float angleDiff = Random.Range (-stability,stability);
				transform.right = Quaternion.AngleAxis(angleDiff,Vector3.forward)*rigidbody2D.velocity;
			}
			rigidbody2D.velocity = transform.right*speed;
		}

		transform.right = rigidbody2D.velocity.normalized;
	}

	void EnableTracking()
	{
		tracking = true;
	}

	Transform findTarget()
	{
		GameObject nearest = ActiveEnemiesSingleton.Instance.GetClosestEnemy(transform.position);
		if(nearest == null)
			return null;
		else
			return nearest.transform;
	}
}

