using UnityEngine;
using System.Collections;

public class Sprint : Buff
{

	//TODO current effect is not symmetrical as a debuff
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

