using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BlastController : MonoBehaviour 
{

	public float existanceTime;
	public Sprite preBlastSprite;
	public Sprite blastSprite;
	Action<Transform, Transform> onHitByBlast;
	LayerMask blastTargets;
	float delay = 0f;
	Vector3 originalScale;
	Collider2D[] colliders;

	void OnEnable()
	{
		if(colliders == null)
			colliders = GetComponents<Collider2D>();
		originalScale = transform.localScale;

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

	void OnTriggerEnter2D(Collider2D col)
	{

		if(onHitByBlast != null && (blastTargets.value &1 << col.gameObject.layer) != 0) {
			onHitByBlast(transform, col.transform);
		}

	}


}
