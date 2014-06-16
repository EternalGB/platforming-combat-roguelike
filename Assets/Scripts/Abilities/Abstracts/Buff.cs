using UnityEngine;
using System.Collections;

public abstract class Buff : Ability
{

	public float buffDuration;

	protected abstract void buffEffect(GameActor actor);

	protected abstract void undoBuff(GameActor actor);

	override sealed public void activeEffect(Transform player)
	{
		GameActor ga = player.GetComponent<GameActor>();
		buffEffect(ga);
		if(buffDuration > 0) {
			StartCoroutine(Timers.Countdown<GameActor>(buffDuration,undoBuff,ga));
		}
	}

	override protected void upgradeOtherAbility(Ability other)
	{
		if(other.GetType().BaseType == typeof(ProjectileAttack)) {
			
		} else if(other.GetType().BaseType == typeof(CloseBlast)) {
			
		} else if(other.GetType().BaseType == typeof(Buff)) {
			
		} else if(other.GetType().BaseType == typeof(Special)) {
			
		}
	}


}

