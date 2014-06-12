using UnityEngine;
using System.Collections;

public class Heal : Buff
{

	override protected void buffEffect(GameActor actor)
	{
		actor.currentHealth = Mathf.Clamp (actor.currentHealth + effectSize,0,actor.maxHealth);
	}

	override protected void undoBuff(GameActor actor)
	{

	}
	

	override protected void upgradeOtherAbility(Ability other)
	{

	}

	override public void passiveEffect(Transform player)
	{

	}

}

