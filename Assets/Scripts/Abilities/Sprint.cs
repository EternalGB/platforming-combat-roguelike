using UnityEngine;
using System.Collections;

public class Sprint : Buff
{

	override public void buffEffect(Transform applier, Transform target)
	{
		GameActor actor = target.GetComponent<GameActor>();
		actor.maxSpeed = actor.maxSpeed + effectSize;
	}

	override public void undoBuff(Transform applier, Transform target)
	{
		GameActor actor = target.GetComponent<GameActor>();
		actor.maxSpeed = actor.maxSpeed - effectSize;
	}
	


	override protected void reset()
	{
		
	}

	override public void passiveEffect(Transform player)
	{

	}

}

