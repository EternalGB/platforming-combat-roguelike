using UnityEngine;
using System.Collections;

public class DamageBlast : CloseBlast
{

	override public void burstEffect(Transform blast, Transform target)
	{
		GameActor actor;
		if(actor = target.GetComponent<GameActor>()) {
			actor.Damage(effectSize);
		}
	}

}

