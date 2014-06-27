using UnityEngine;
using System.Collections;

public class ChanceOnHit : MonoBehaviour
{

	System.Action<Transform, Transform> onHit;
	LayerMask triggerLayers;
	float chance;
	
	void OnCollisionEnter2D(Collision2D col)
	{
		if(onHit != null && (triggerLayers.value &1 << col.gameObject.layer) != 0 && Random.value < chance) {
			print("Firing onHit from " + transform + " at " + transform.position);
			onHit(transform, col.transform);
		}
	}
	
	public void init(System.Action<Transform, Transform> action, LayerMask hitTriggers, float chance)
	{
		onHit = action;
		this.triggerLayers = hitTriggers;
		this.chance = chance;
	}
	

}

