using UnityEngine;
using System.Collections;

public class FlyingGuard : BaseEnemyBehaviour
{
	
	public Transform tether;
	public bool withinTetherRadius;
	public float patrolInterval;
	float tetherRadius;
	public LayerMask targets;
	public float chaseRadius;
	Transform target;
	Vector3 dest;

	//TODO can't path around obstacles
	
	void Start ()
	{
		base.Start ();
		dest = transform.position;
	}


	void FixedUpdate ()
	{

		withinTetherRadius = checkTether();
		//print(withinTetherRadius);

		Color lineColor = Color.green;

		if((target = getTarget())) {
			dest = target.position;
			lineColor = Color.red;
			CancelInvoke("GetRandomDest");
		} else if(!withinTetherRadius) {
			dest = tether.position;
			CancelInvoke("GetRandomDest");
		} else if(Vector2.Distance(transform.position,dest) <= 0.3) {
			Invoke("GetRandomDest",0);
		} else {
			if(!IsInvoking("GetRandomDest"))
				Invoke("GetRandomDest",patrolInterval);
		}

		Debug.DrawLine(transform.position,dest,lineColor);
		//transform.position += (dest - transform.position).normalized*maxSpeed*Time.fixedDeltaTime;
		base.FixedUpdate();
	}

	override protected void physicsMove(Vector2 moveDir, float accel)
	{
		Vector2 newVel = rigidbody2D.velocity + moveDir*accel*Time.fixedDeltaTime;
		if((newVel.magnitude <= maxSpeed)
		   || (rigidbody2D.velocity.magnitude > maxSpeed && newVel.magnitude < rigidbody2D.velocity.magnitude))
		{
			rigidbody2D.velocity = newVel;
		}
	}


	bool checkTether()
	{
		return Vector2.Distance(transform.position,tether.position) <= tetherRadius;
	}

	Transform getTarget()
	{
		Collider2D detected = Physics2D.OverlapCircle(transform.position,chaseRadius,targets);
		if(detected != null)
			return detected.transform;
		else
			return null;
	}

	void GetRandomDest()
	{
		dest = (Vector2)tether.position + Random.insideUnitCircle*tetherRadius;
	}

	public void SetTether(Transform tether, float radius)
	{
		this.tether = tether;
		tetherRadius = radius;
	}


	override protected Vector2 movingDir()
	{
		return (dest - transform.position).normalized;
	}
	
	override protected bool isStrafing()
	{
		return false;
	}
}

