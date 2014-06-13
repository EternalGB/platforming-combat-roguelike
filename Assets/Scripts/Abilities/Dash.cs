using UnityEngine;
using System.Collections;

public class Dash : Ability
{
	

	override public void activeEffect(Transform player)
	{
		player.SendMessage("Dash",effectSize);
	}
	
	override protected void upgradeOtherAbility(Ability other)
	{
		//if the ability is not a direct child of the ability class
		//then is may have some other base type that we have to check
		if(other.GetType().BaseType != typeof(Ability)) {
			if(other.GetType().BaseType == typeof(ProjectileAttack)) {
				upgradeProjectileAttack((ProjectileAttack)other);
			}
		//else it's an instance of a direct child class
		} else if(other.GetType () == typeof(ProjectileAttack)) {
			upgradeProjectileAttack((ProjectileAttack)other);
		}
	}

	private void upgradeProjectileAttack(ProjectileAttack pa)
	{
		pa.bulletVelocity += effectSize;
	}

	override protected void reset()
	{
		
	}

	override public void passiveEffect(Transform player)
	{

	}

}

