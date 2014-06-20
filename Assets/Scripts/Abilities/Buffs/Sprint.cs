using UnityEngine;
using System.Collections;

public class Sprint : Buff
{

	override public void buffEffect(Transform applier, Transform target)
	{
		float scaling;
		if(effectSize > 0)
			scaling = effectSize;
		else
			scaling = 1/-effectSize;

		GameActor actor;
		if(actor = target.GetComponent<GameActor>())
			actor.maxSpeed *= scaling;
	}

	override public void undoBuff(Transform applier, Transform target)
	{

		float scaling;
		if(effectSize > 0)
			scaling = effectSize;
		else
			scaling = 1/-effectSize;

		GameActor actor = target.GetComponent<GameActor>();
		actor.maxSpeed /= scaling;
	}
	
	

	override public void passiveEffect(Transform player)
	{

	}

}

