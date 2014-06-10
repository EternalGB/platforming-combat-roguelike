using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : GameActor 
{

	float hori = 0f;
	float vert = 0f;
	float strafe = 0f;

	public Animator anim;


	public float jumpPower;
	public float jumpDuration;
	float jumpTimeStart;





	bool onGround = false;
	public Transform groundCheck;
	float groundCheckRadius = 0.2f;
	public float baseGroundFriction;
	public LayerMask groundLayer;


	void Start()
	{
		base.Start();
		jumpTimeStart = -1;



	}

	void Update()
	{
		hori = Input.GetAxis ("Horizontal");
		vert = Input.GetAxisRaw ("Vertical");
		strafe = Input.GetAxisRaw ("Strafe");
	}

	override protected float horizontalMovingDir()
	{
		return hori;
	}

	override protected bool isStrafing()
	{
		return strafe > 0;
	}

	void FixedUpdate()
	{

		anim.SetFloat("horiSpeed",Mathf.Abs(hori));
		onGround = Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius,groundLayer);
		anim.SetBool("onGround",onGround);

		if(onGround) {
			if(vert > 0) {
				onGround = false;
				rigidbody2D.AddForce(new Vector2(0,jumpPower));
				jumpTimeStart = Time.time;
			} else {
				DoFriction(rigidbody2D,baseGroundFriction,maxSpeed);

			}
		}

		if(vert > 0 && jumpTimeStart > 0 && Time.time - jumpTimeStart <= jumpDuration) {
			rigidbody2D.AddForce (new Vector2(0,2*jumpPower*Time.fixedDeltaTime));
		}

		base.FixedUpdate();
	}

	static void DoFriction(Rigidbody2D body, float power, float max)
	{
		Vector2 final = new Vector2(body.velocity.x,body.velocity.y);
		if(body.velocity.x < 0) {
			final.x = Mathf.Clamp(final.x + power,-max,0);
		} else if(body.velocity.x > 0) {
			final.x = Mathf.Clamp(final.x - power,0,max);
		}
		body.velocity = final;
	}
	





}
