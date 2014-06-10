using UnityEngine;
using System.Collections;

public class PlatformGuard : GameActor
{

	public enum BehaviourState
	{CHASE,PATROL};

	float movingDir = -1;
	float patrolDir = -1;
	BehaviourState behaviour;
	Transform target;
	public Transform forwardGroundCheck;
	bool groundInFront = false;
	float groundCheckRadius = 0.2f;
	public LayerMask groundLayer;
	public float detectionRange;
	public LayerMask potentialTargets;
	public Animator anim;

	void Start()
	{
		base.Start ();
		horiAcceleration = maxSpeed/10;
		behaviour = BehaviourState.PATROL;
		facingRight = false;
	}

	//TODO prevent from chasing off of platform boundaries
	void FixedUpdate()
	{
		groundInFront = Physics2D.OverlapCircle(forwardGroundCheck.position,groundCheckRadius,groundLayer);
		float movementPower = horiAcceleration;

		if((target = getTarget())) {
			behaviour = BehaviourState.CHASE;

		} else {
			behaviour = BehaviourState.PATROL;
		}

		if(behaviour == BehaviourState.PATROL) {
			if(anim != null)
				anim.SetBool("walking",true);
			if(!groundInFront) {
				patrolDir = -patrolDir;
			}
			movingDir = patrolDir;
		} else if(behaviour == BehaviourState.CHASE) {
			if(anim != null)
				anim.SetBool("walking",true);
			movingDir = (target.position - transform.position).x;
			if(!groundInFront) {
				if(anim != null)
					anim.SetBool("walking",false);
				movementPower = 0;
				//rigidbody2D.velocity = new Vector2(0,rigidbody2D.velocity.y);
			}
		}
		horizontalPhysicsMovement(movingDir,movementPower);
		base.FixedUpdate();
	}

	Transform getTarget()
	{
		Collider2D detected = Physics2D.OverlapCircle(transform.position,detectionRange,potentialTargets);
		if(detected != null)
			return detected.transform;
		else
			return null;
	}



	override protected float horizontalMovingDir()
	{
		return movingDir;
	}
	
	override protected bool isStrafing()
	{
		return false;
	}

}

