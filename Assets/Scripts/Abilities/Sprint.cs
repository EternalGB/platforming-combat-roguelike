using UnityEngine;
using System.Collections;

public class Sprint : Buff
{

	override protected void buffEffect(GameActor actor)
	{
		actor.maxSpeed = actor.maxSpeed + effectSize;
	}

	override protected void undoBuff(GameActor actor)
	{
		actor.maxSpeed = actor.maxSpeed - effectSize;
	}
	

	override protected void upgradeOtherAbility(Ability other)
	{

	}

	override protected void reset()
	{
		
	}

	override public void passiveEffect(Transform player)
	{

	}

}

