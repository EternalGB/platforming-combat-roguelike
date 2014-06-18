using UnityEngine;
using System.Collections;

public class Glide : Special
{

	bool gliding = false;
	float savedGravity;
	Transform glidingPlayer;

	override public void activeEffect(Transform player)
	{
		if(!gliding) {
			gliding = true;
			savedGravity = player.rigidbody2D.gravityScale;
			glidingPlayer = player;
			player.rigidbody2D.gravityScale = 1;
			StartCoroutine(Timers.Countdown(effectSize,stopGlide));
		} else {
			gliding = false;
		}
	}

	void stopGlide()
	{
		glidingPlayer.rigidbody2D.gravityScale = savedGravity;
		gliding = false;
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
		pa.rigidbody2D.gravityScale = 1;
	}

	override protected void reset()
	{
		
	}

	override public void passiveEffect(Transform player)
	{
		
	}

}

