using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : GameActor 
{

	float hori = 0f;
	float vert = 0f;

	public Animator anim;
	
	public float jumpPower;
	public float jumpDuration;
	float jumpTimeStart;

	public bool onClimbable = false;
	public LayerMask climbableLayer;
	bool firstClimbingStep = true;

	public int xp;


	
	public static GameObject GlobalPlayerInstance
	{
		get;
		private set;
	}

	void Awake()
	{
		if(GlobalPlayerInstance != null && GlobalPlayerInstance != this) {
			Destroy(gameObject);
			
		}
		
		GlobalPlayerInstance = gameObject;
	}

	void Start()
	{
		base.Start();
		jumpTimeStart = -1;

	}

	void Update()
	{


		hori = Input.GetAxis ("Horizontal");
		vert = Input.GetAxis ("Vertical");
		if(onGround && !onClimbable && Input.GetButtonDown("Jump")) {
			//TODO should only be able apply the jump force exactly once
			onGround = false;
			rigidbody2D.AddForce(new Vector2(0,jumpPower));
			jumpTimeStart = Time.time;				 
		}

		onClimbable = Physics2D.OverlapCircle (transform.position, 0.5f, climbableLayer);
		if (onClimbable && Mathf.Abs (vert) > 0) {
			rigidbody2D.gravityScale = 0;
		} else {
			rigidbody2D.gravityScale = savedGravity;
			firstClimbingStep = true;
		}

		anim.SetFloat("horiSpeed",Mathf.Abs(hori));
		anim.SetBool("onGround",onGround);

	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere (transform.position, 0.5f);
	}

	override protected Vector2 movingDir()
	{
		return new Vector2(hori,0);
	}

	override protected bool isStrafing()
	{
		return Input.GetButton("Strafe");
	}

	void FixedUpdate()
	{


		base.FixedUpdate();
		if(Input.GetButton ("Jump") && jumpTimeStart > 0 && Time.time - jumpTimeStart <= jumpDuration) {
			rigidbody2D.AddForce (new Vector2(0,2*jumpPower*Time.fixedDeltaTime));
		}


	}

	protected override void movementDecision ()
	{
		if (onClimbable && Mathf.Abs(vert) > 0) {
			if(firstClimbingStep) {
				rigidbody2D.velocity = Vector2.zero;
				firstClimbingStep = false;
			}
			transform.position += (new Vector3(hori,vert))
			                       *(maxSpeed/2)*Time.fixedDeltaTime;
			anim.SetBool("climbing",true);
		} else  {
			base.movementDecision ();
			anim.SetBool("climbing",false);
		}
	}

	override public void Die()
	{
		hori = 0;
		rigidbody2D.velocity = Vector2.zero;
		currentHealth = maxHealth;
		transform.position = GameObject.Find("PlayerSpawn").transform.position;
	}

	public void GetXP(int amount)
	{
		xp += amount;
	}



}
