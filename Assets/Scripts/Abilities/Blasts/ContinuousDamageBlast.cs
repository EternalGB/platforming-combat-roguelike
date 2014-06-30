using UnityEngine;
using System.Collections;

public class ContinuousDamageBlast : CloseBlast
{

	override public void burstEffect(Transform blast, Transform target)
	{
		GameActor actor;
		if(actor = target.GetComponent<GameActor>()) {
			actor.Damage(effectSize*Time.deltaTime);
		}
	}

}

