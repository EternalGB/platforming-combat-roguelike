using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class Buff : Ability
{

	public float buffDuration;
	Dictionary<Action<Transform, Transform>,Action<Transform, Transform> > buffs;
	Dictionary<Action<Transform, Transform>, float> buffDurations;

	private float origBuffDuration;

	public void Start()
	{
		base.Start();
		origBuffDuration = buffDuration;
		initBuffs();
		if(upgrade != null) {
			upgradeAbility(upgrade);
		}
	}

	void initBuffs()
	{
		buffs = new Dictionary<Action<Transform, Transform>, Action<Transform, Transform>>();
		buffDurations = new Dictionary<Action<Transform, Transform>, float>();
		buffs.Add(buffEffect, undoBuff);
		buffDurations.Add(buffEffect, buffDuration);
	}

	public abstract void buffEffect(Transform applier, Transform target);

	public abstract void undoBuff(Transform applier, Transform target);

	override sealed public void activeEffect(Transform player)
	{
		foreach(Action<Transform, Transform> buff in buffs.Keys) {
			doBuff(buff,buffs[buff],player,player, buffDurations[buff]);
		}
	}

	void doBuff(Action<Transform, Transform> buffEffect, Action<Transform, Transform> undoBuff,
	            Transform applier, Transform target, float duration)
	{
		buffEffect(applier,target);
		if(duration > 0) {
			StartCoroutine(Timers.Countdown<Transform,Transform>(duration,undoBuff,applier,target));
		}
	}

	void doBuff(Transform applier, Transform target)
	{
		buffEffect(applier,target);
		if(buffDuration > 0) {
			StartCoroutine(Timers.Countdown<Transform,Transform>(buffDuration,undoBuff,applier,target));
		}
	}

	override protected void upgradeOtherAbility(Ability other)
	{
		print(abilityName + " upgrading " + other.abilityName);
		if(other.GetType().BaseType == typeof(ProjectileAttack)) {
			ProjectileAttack pa = (ProjectileAttack)other;
			effectSize = -effectSize;
			pa.onCollision = doBuff;
		} else if(other.GetType().BaseType == typeof(CloseBlast)) {
			CloseBlast cb = (CloseBlast)other;
			effectSize = -effectSize;
			cb.onHitByBurst = doBuff;
		} else if(other.GetType().BaseType == typeof(Buff)) {
			Buff b = (Buff)other;
			b.buffs.Add(buffEffect,undoBuff);
			b.buffDurations.Add(buffEffect,buffDuration);
		} else if(other.GetType().BaseType == typeof(Special)) {
			if(other.GetType() == typeof(Dash)) {
				Dash d = (Dash)other;
				d.preDashAction = activeEffect;
			} else if(other.GetType() == typeof(ClusterShower)) {
				ClusterShower cs = (ClusterShower)other;
				effectSize = -effectSize;
				cs.onCollision = doBuff;
			}
		}
	}

	override protected void reset()
	{
		buffDuration = origBuffDuration;
		initBuffs();
	}


}

