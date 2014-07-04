using UnityEngine;
using System.Collections;

public class LazorBlast : CloseBlast
{
	
	override public void burstEffect(Transform blast, Transform target)
	{
		GameActor actor;
		if(actor = target.GetComponent<GameActor>()) {
			actor.Damage(effectSize);
		}
	}

	protected override void scaleBlast (GameObject blast, float scaling)
	{
		Vector3 scale = blast.transform.localScale;
		blast.transform.localScale = new Vector3(scaling*scale.x,1*scale.y);
	}
	
}

