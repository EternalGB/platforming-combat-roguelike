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
	int passiveOrbCounter = 0;

	Action<Transform, Transform> onCollision;
	public LayerMask onCollisionTargets;
	
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
		}

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
		throw new System.NotImplementedException ();
	}

	protected override void reset ()
	{
		throw new System.NotImplementedException ();
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







}

