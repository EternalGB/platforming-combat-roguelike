using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class GameActor : MonoBehaviour
{

	Color defaultColor;
	public float maxSpeed;
	protected float acceleration;
	public bool dashing = false;
	float dashPower;
	protected float savedGravity = 1;
	protected bool facingRight = true;
	public Texture healthBar;
	public Texture healthBarBacking;
	public bool drawLocalHealthBar = true;
	public float currentHealth;
	public float maxHealth;
	public float healthRegen;
	const float regenInterval = 0.5f;
	const float globalMaxSpeed = 30;
	List<DamageOverTime> dots;
	protected SpriteRenderer spriteRenderer;

	float dmgTakenMulti = 1;

	protected bool collidingWithGround = false;
	protected bool onGround = false;
	public Transform groundCheck;
	protected float groundCheckRadius = 0.3f;
	public LayerMask groundLayer;



	public Vector2 facingDir
	{
		get
		{
			return transform.right * Mathf.Sign (transform.localScale.x);
		}
	}



	protected void Flip()
	{
		facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	protected void Start()
	{
		InvokeRepeating("Regen",0,regenInterval);
		spriteRenderer = GetComponent<SpriteRenderer>();
		defaultColor = spriteRenderer.color;
		acceleration = maxSpeed;
		savedGravity = rigidbody2D.gravityScale;

	}

	protected void FixedUpdate()
	{
		if(groundCheck != null) {
			onGround = Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius,groundLayer);

			//TODO one way platforms
			//Physics2D.IgnoreLayerCollision(gameObject.layer,LayerMask.NameToLayer("Ground"),
			//                               collidingWithGround && !onGround);
		}



		if(!isStrafing()) {
			if(movingDir().x > 0 && !facingRight)
				Flip();
			else if(movingDir().x < 0 && facingRight)
				Flip();
		}

		if(dots == null)
			dots = new List<DamageOverTime>();
		for(int i = 0; i < dots.Count; i++) {
			if(dots[i].isStopped()) {
				dots.RemoveAt(i);
				i--;
			}
		}

		if(dots.Count == 0)
			GetComponent<SpriteRenderer>().color = defaultColor;


		movementDecision ();
		/*
		 * This way the old way of doing it. Setting velocity to zero = bad
		if(groundCheck && collidingWithGround && !onGround) {
			rigidbody2D.velocity = new Vector2(0,rigidbody2D.velocity.y);
		}
		*/

		rigidbody2D.velocity = Vector2.ClampMagnitude(rigidbody2D.velocity,globalMaxSpeed);

		if(currentHealth <= 0)
			Die();
	}

	protected virtual void movementDecision()
	{
		if (dashing) {
			rigidbody2D.velocity = dashPower * facingDir;
		} else if((!collidingWithGround || onGround) || !groundCheck) {
			//doing it this way allows for other physics forces to affect the player
			physicsMove(movingDir(),acceleration);
			
		}
	}

	protected virtual void physicsMove(Vector2 moveDir, float accel)
	{
		if((moveDir.magnitude > 0 && Mathf.Abs(rigidbody2D.velocity.x) < maxSpeed)
		   || (rigidbody2D.velocity.x > maxSpeed && moveDir.x < 0)
		   || (rigidbody2D.velocity.x < -maxSpeed && moveDir.x > 0)) 
		{
			rigidbody2D.velocity = rigidbody2D.velocity + moveDir*accel;
			//rigidbody2D.velocity = 
			//	new Vector2(rigidbody2D.velocity.x + moveDir.x*acceleration, rigidbody2D.velocity.y + moveDir.y*acceleration);
		}
	}

	public void SetColor(Color color)
	{
		GetComponent<SpriteRenderer>().color = color;
	}

	protected void ApplyDOT(Vector2 values)
	{
		ApplyDOT(values.x,(int)values.y);
	}

	public void ApplyDOT(float amount, int intervals)
	{
		DamageOverTime dot = new DamageOverTime(intervals,amount,Damage);
		StartCoroutine(dot.Start());
		dots.Add(dot);
	}

	protected void RemoveAllDOT()
	{
		foreach(DamageOverTime dot in dots)
			dot.Stop();
		dots.Clear();
	}

	public void SetDamageTakenMultiplier(float multiplier)
	{
		dmgTakenMulti = multiplier;
	}

	public void Damage(float amount)
	{
		currentHealth -= dmgTakenMulti*amount;
	}

	public virtual void Die()
	{
		GameObject.Destroy(gameObject);
	}

	protected void Regen()
	{
		currentHealth = Mathf.Clamp(currentHealth + healthRegen,-maxHealth,maxHealth);
	}

	protected void Dash(float dashPower)
	{
		dashing = true;
		this.dashPower = dashPower;

		savedGravity = rigidbody2D.gravityScale;
		rigidbody2D.gravityScale = 0;
		StartCoroutine(Timers.Countdown(0.05f,EndDash));
	}

	void EndDash()
	{
		rigidbody2D.gravityScale = savedGravity;
		rigidbody2D.velocity = Vector2.zero;
		dashing = false;
	}

	void OnGUI()
	{
		if(!PlayerGUI.paused && drawLocalHealthBar && currentHealth < maxHealth)
			DrawLocalHealthBar();
	}

	protected void DrawLocalHealthBar()
	{
		Bounds bounds = spriteRenderer.bounds;

		Vector3 topLeft = Camera.main.WorldToScreenPoint(new Vector2(bounds.min.x,bounds.max.y));
		Vector3 topRight = Camera.main.WorldToScreenPoint(new Vector2(bounds.max.x,bounds.max.y));
		float width = topRight.x - topLeft.x;
		float height = 5;
		float padding = 5;
		float pHealth = currentHealth/maxHealth;
		GUI.DrawTexture(new Rect(topLeft.x,Screen.height - (topLeft.y + height + padding),width,height),healthBarBacking);
		GUI.DrawTexture(new Rect(topLeft.x,Screen.height - (topLeft.y + height + padding),
		                         width*pHealth,height),healthBar);

	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer("KillTriggers")) {
			Die();
		} 
	}
	
	void OnCollisionStay2D(Collision2D col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer("Ground"))
			collidingWithGround = true;
	}

	void OnCollisionExit2D(Collision2D col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer("Ground"))
			collidingWithGround = false;
	}

	protected abstract Vector2 movingDir();

	protected abstract bool isStrafing();

}

