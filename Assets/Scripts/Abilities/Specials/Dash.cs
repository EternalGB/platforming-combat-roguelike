using UnityEngine;
using System.Collections;

public class Dash : Special
{

	float knockbackScaling = 5;
	float speedScaling = 5;
	public System.Action<Transform> preDashAction;



	override public void activeEffect(Transform player)
	{
		if(player.GetComponent<PlayerController>()) {
			if(preDashAction != null) 
				preDashAction(player);
			player.SendMessage("Dash",effectSize);
		}
	}
	
	override protected void upgradeOtherAbility(Ability other)
	{
		if(other.GetType().BaseType == typeof(ProjectileAttack)) {
			ProjectileAttack pa = (ProjectileAttack)other;
			pa.bulletVelocity += speedScaling*effectSize;
		} else if(other.GetType().BaseType == typeof(CloseBlast)) {
			CloseBlast cb = (CloseBlast)other;
			cb.onHitByBurst = knockback;
		} else if(other.GetType().BaseType == typeof(Buff)) {
			Buff b = (Buff)other;
			b.effectSize += effectSize/5;
		} else if(other.GetType().BaseType == typeof(Special)) {
			if(other.GetType() == typeof(ClusterShower)) {
				ClusterShower cs = (ClusterShower)other;
				cs.onCollision = knockback;
			}
		}
	}

	void knockback(Transform applier, Transform target)
	{
		if(target.rigidbody2D != null) {
			Vector3 forceDir = target.position - applier.position;
			Vector3 force = forceDir.normalized*effectSize*knockbackScaling;
			target.rigidbody2D.AddForce(force);
		}
	}

	override protected void reset()
	{
		preDashAction = null;
	}

	override public void passiveEffect(Transform player)
	{

	}

}

