using UnityEngine;
using System.Collections;

public class PlatformGuard : BaseEnemyBehaviour
{

	public enum BehaviourState
	{CHASE,PATROL};

	float travelDir = -1;
	float patrolDir = -1;
	BehaviourState behaviour;
	Transform target;
	public Transform forwardGroundCheck;
	bool groundInFront = false;

	public float detectionRange;
	public LayerMask potentialTargets;
	public Animator anim;



	void Start()
	{
		base.Start ();
		behaviour = BehaviourState.PATROL;
		facingRight = false;
	}

	//TODO prevent from chasing off of platform boundaries
	void FixedUpdate()
	{
		base.FixedUpdate();

		groundInFront = Physics2D.OverlapCircle(forwardGroundCheck.position,groundCheckRadius,groundLayer);



		float savedAccel = acceleration;

		if((target = getTarget())) {
			behaviour = BehaviourState.CHASE;

		} else {
			behaviour = BehaviourState.PATROL;
		}

		if(behaviour == BehaviourState.PATROL) {
			if(anim != null)
				anim.SetBool("walking",true);
			if(!groundInFront && onGround) {
				patrolDir = -patrolDir;
			}
			travelDir = patrolDir;
		} else if(behaviour == BehaviourState.CHASE) {
			if(anim != null)
				anim.SetBool("walking",true);
			travelDir = (target.position - transform.position).x;
			if(!groundInFront && onGround) {
				if(anim != null)
					anim.SetBool("walking",false);
				acceleration = 0;
				//rigidbody2D.velocity = new Vector2(0,rigidbody2D.velocity.y);
			}
		}
		//horizontalPhysicsMovement(travelDir,movementPower);


		acceleration = savedAccel;
	}

	Transform getTarget()
	{
		Collider2D detected = Physics2D.OverlapCircle(transform.position,detectionRange,potentialTargets);
		if(detected != null)
			return detected.transform;
		else
			return null;
	}




	override protected Vector2 movingDir()
	{
		return new Vector2(travelDir,0);
	}
	
	override protected bool isStrafing()
	{
		return false;
	}

}

