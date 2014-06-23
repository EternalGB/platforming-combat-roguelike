using UnityEngine;
using System.Collections;


public class ActionOnHit : MonoBehaviour
{

	System.Action<Transform, Transform> onHit;
	LayerMask triggerLayers;
	float cooldown;
	bool canActivate = true;

	void OnCollisionEnter2D(Collision2D col)
	{
		if(onHit != null && canActivate && (triggerLayers.value &1 << col.gameObject.layer) != 0) {
			print("Firing onHit from " + transform + " at " + transform.position);
			onHit(transform, col.transform);
			canActivate = false;
			StartCoroutine(Timers.Countdown(cooldown, enableActivate));
		}
	}

	public void init(System.Action<Transform, Transform> action, LayerMask hitTriggers, float cooldown)
	{
		onHit = action;
		this.triggerLayers = hitTriggers;
		this.cooldown = cooldown;
	}

	void enableActivate()
	{
		canActivate = true;
	}



}

