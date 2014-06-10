using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class GameActor : MonoBehaviour
{

	Color defaultColor;
	public float maxSpeed;
	protected float horiAcceleration;
	public bool dashing = false;
	float dashPower;
	float savedGravity = 1;
	protected bool facingRight = true;
	public Texture healthBar;
	public Texture healthBarBacking;
	public bool drawLocalHealthBar = true;
	public float currentHealth;
	public float maxHealth;
	public float healthRegen;
	const float regenInterval = 0.5f;
	const float globalMaxSpeed = 18;
	List<DamageOverTime> dots;
	protected SpriteRenderer spriteRenderer;


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
		horiAcceleration = maxSpeed/5;
		savedGravity = rigidbody2D.gravityScale;

	}

	protected void FixedUpdate()
	{
		if(!isStrafing()) {
			if(horizontalMovingDir() > 0 && !facingRight)
				Flip();
			else if(horizontalMovingDir() < 0 && facingRight)
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

		if(dashing) {

			rigidbody2D.velocity = dashPower*facingDir;
			//doing it this way allows for other physics forces to affect the player
		} else {
			horizontalPhysicsMovement(horizontalMovingDir(),horiAcceleration);
			rigidbody2D.velocity = Vector2.ClampMagnitude(rigidbody2D.velocity,globalMaxSpeed);
		}




		if(currentHealth <= 0)
			Die();
	}

	protected void horizontalPhysicsMovement(float moveDir, float acceleration)
	{
		if((Mathf.Abs(moveDir) > 0 && Mathf.Abs(rigidbody2D.velocity.x) < maxSpeed)
		   || (rigidbody2D.velocity.x > maxSpeed && moveDir < 0)
		   || (rigidbody2D.velocity.x < -maxSpeed && moveDir > 0)) 
		{
			rigidbody2D.velocity = 
				new Vector2(rigidbody2D.velocity.x + moveDir*acceleration, rigidbody2D.velocity.y);
		}
	}

	protected void SetColor(Color color)
	{
		GetComponent<SpriteRenderer>().color = color;
	}

	protected void ApplyDOT(Vector2 values)
	{
		ApplyDOT(values.x,(int)values.y);
	}

	protected void ApplyDOT(float amount, int intervals)
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

	protected void Damage(float amount)
	{
		currentHealth -= amount;
	}

	protected void Die()
	{
		Destroy(gameObject);
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
		if(drawLocalHealthBar && currentHealth < maxHealth)
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

	protected abstract float horizontalMovingDir();

	protected abstract bool isStrafing();

}

