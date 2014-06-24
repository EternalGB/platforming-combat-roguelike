using UnityEngine;
using System.Collections;

public class Dash : Special
{

	float knockbackScaling = 5;
	float speedScaling = 5;
	public System.Action<Transform> preDashAction;
	public LayerMask onHitPassiveTargets;
	int passiveActionID = 0;

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

	void speedBoost(Transform t1, Transform t2)
	{
		GameActor actor = t1.GetComponent<GameActor>();
		actor.maxSpeed += effectSize/10;
		StartCoroutine(Timers.Countdown<Transform>(cooldown/2,removeSpeedBoost,t1));
	}

	void removeSpeedBoost(Transform t1)
	{
		GameActor actor = t1.GetComponent<GameActor>();
		actor.maxSpeed -= effectSize/10;
	}

	override public void applyPassive(Transform player)
	{
		ActionOnHit action = player.gameObject.AddComponent<ActionOnHit>();
		action.init(speedBoost,onHitPassiveTargets,cooldown);
		passiveActionID = action.GetInstanceID();
	}
	
	override public void undoPassive(Transform player)
	{
		ActionOnHit[] actions = player.gameObject.GetComponents<ActionOnHit>();
		ActionOnHit action = null;
		foreach(ActionOnHit a in actions)
			if(a.GetInstanceID() == passiveActionID)
				action = a;
		if(action)
			Destroy(action);
	}

}

