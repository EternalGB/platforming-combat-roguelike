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

	// Use this for initialization
	void Start ()
	{
		tetherRadius = tether.GetComponent<CircleCollider2D>().radius;
		dest = transform.position;
		base.Start ();
	}

	// Update is called once per frame
	void FixedUpdate ()
	{

		if((target = getTarget())) {
			dest = target.position;
			CancelInvoke("GetRandomDest");
		} else if(!withinTetherRadius) {
			dest = tether.position;
			CancelInvoke("GetRandomDest");
		} else if(Vector3.Distance(transform.position,dest) <= 0.1) {
			Invoke("GetRandomDest",0);
		} else {
			if(!IsInvoking("GetRandomDest"))
				Invoke("GetRandomDest",patrolInterval);
		}


		transform.position += (dest - transform.position).normalized*maxSpeed*Time.fixedDeltaTime;
		base.FixedUpdate();
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if(other.transform.GetInstanceID() == tether.GetInstanceID())
			withinTetherRadius = true;
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

	void SetTether(Transform tether)
	{
		this.tether = tether;
	}


	override protected float horizontalMovingDir()
	{
		return (transform.position- dest).x;
	}
	
	override protected bool isStrafing()
	{
		return false;
	}
}

