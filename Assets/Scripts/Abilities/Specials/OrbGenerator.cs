using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class OrbGenerator : Special
{

	public GameObject orbObj;



	List<OrbController> orbs;
	public int maxOrbs;
	float interOrbAngle;
	float orbRadius;
	float orbSpeed;

	public Action<Transform, Transform> onCollision;
	public LayerMask onCollisionTargets;
	
	Action<Transform, Transform> origOnCollision;
	LayerMask origOnCollisionTargets;
	float origRadius;
	float origSpeed;

	void Start()
	{
		onCollision = defaultCollision;
		orbs = new List<OrbController>();
		if(maxOrbs > 0)
			interOrbAngle = 360/maxOrbs;
		for(int i = 0; i < maxOrbs; i++) {
			OrbController oc = ((GameObject)GameObject.Instantiate(orbObj)).GetComponent<OrbController>();
			oc.offset = Quaternion.AngleAxis(i*interOrbAngle,Vector3.forward)*Vector3.right;
			oc.owner = PlayerController.GlobalPlayerInstance.transform;
			oc.SetOnCollision(onCollision,onCollisionTargets);
			orbs.Add(oc);
			orbRadius = oc.radius;
			orbSpeed = oc.rotationSpeed;
		}
		origOnCollision = onCollision;
		origOnCollisionTargets = onCollisionTargets;
		origRadius = orbRadius;
		origSpeed = orbSpeed;
		base.Start ();
	}

	public override void activeEffect (Transform player)
	{
		foreach(OrbController orb in orbs)
			orb.TurnOn();
	}
	

	void defaultCollision(Transform applier, Transform target)
	{
		if(target.GetComponent<GameActor>())
			target.SendMessage("Damage",effectSize);
	}


	protected override void upgradeOtherAbility (Ability other)
	{
		if(other.GetType().BaseType == typeof(ProjectileAttack)) {
			ProjectileAttack pa = (ProjectileAttack)other;
			pa.onCollision = defaultCollision;
		} else if(other.GetType().BaseType == typeof(CloseBlast)) {
			CloseBlast cb = (CloseBlast)other;
			cb.onHitByBurst = defaultCollision;
		} else if(other.GetType().BaseType == typeof(Buff)) {
			Buff b = (Buff)other;
			b.effectSize += effectSize/5;
		} else if(other.GetType().BaseType == typeof(Special)) {
			if(other.GetType() == typeof(ClusterShower)) {
				ClusterShower cs = (ClusterShower)other;
				cs.onCollision = defaultCollision;
			} else if(other.GetType() == typeof(Dash)) {
				Dash d = (Dash)other;
				d.preDashAction = activeEffect;
			}
		}
	}

	protected override void reset ()
	{
		onCollision = origOnCollision;
		onCollisionTargets = origOnCollisionTargets;
		orbRadius = origRadius;
		orbSpeed = origSpeed;
		SetOrbRadius(orbRadius);
		SetOrbSpeed(orbSpeed);
	}

	void turnOnNextOrb()
	{
		for(int i = 0; i < maxOrbs; i++) {
			OrbController orb = orbs[i];
			if(orb && !orb.IsOn) {
				orb.TurnOn();
				return;
			}
		}
	}

	IEnumerator passiveCoroutine(float interval)
	{
		for(float timer = interval; timer >= 0; timer -= Time.deltaTime)
			yield return 0;

		turnOnNextOrb();
		StartCoroutine("passiveCoroutine", interval);
	}

	public override void applyPassive (Transform player)
	{
		StartCoroutine("passiveCoroutine", cooldown);
	}

	public override void undoPassive (Transform player)
	{
		StopCoroutine("passiveCoroutine");
	}


	public void SetOrbRadius(float radius)
	{
		foreach(OrbController orb in orbs)
			orb.radius = radius;
	}

	public void SetOrbSpeed(float speed)
	{
		foreach(OrbController orb in orbs)
			orb.rotationSpeed = speed;
	}

	public void IncreaseOrbSpeed(float inc)
	{
		foreach(OrbController orb in orbs)
			orb.rotationSpeed += inc;
	}






}

