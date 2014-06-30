using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class BlastController : MonoBehaviour 
{

	public GameActor owner;
	public float existanceTime;
	public Sprite preBlastSprite;
	public Sprite blastSprite;
	protected Action<Transform, Transform> onHitByBlast;
	protected LayerMask blastTargets;
	protected float delay = 0f;
	protected Vector3 originalScale;
	protected float origDuration;
	protected Collider2D[] colliders;

	void Update()
	{
		if(owner != null) {
			transform.position = owner.transform.position;
			transform.right = owner.facingDir;
		}
	}

	void OnEnable()
	{
		if(colliders == null)
			colliders = GetComponents<Collider2D>();
		originalScale = transform.localScale;
		origDuration = existanceTime;
	}

	void Destroy()
	{
		gameObject.SetActive (false);
	}

	void OnDisable()
	{
		onHitByBlast = null;
		foreach(Collider2D col in colliders)
			col.enabled = false;
		GetComponent<SpriteRenderer>().sprite = preBlastSprite;
		transform.localScale = originalScale;
		existanceTime = origDuration;
		CancelInvoke();
	}

	void SetBlastEffect(UpgradeAction ua)
	{
		onHitByBlast = ua.targetAction;
		blastTargets = ua.targets;
	}

	void StartDelay(float delay)
	{
		this.delay = delay;
		StartCoroutine(Timers.Countdown(delay,EnableBlast));
		Invoke("Destroy",existanceTime + delay);
	}

	void EnableBlast()
	{
		GetComponent<SpriteRenderer>().sprite = blastSprite;
		foreach(Collider2D col in colliders)
			col.enabled = true;
	}

	void SetDuration(float duration)
	{
		existanceTime = duration;
	}

	void SetOwner(GameActor ga)
	{
		this.owner = ga;
	}

}
