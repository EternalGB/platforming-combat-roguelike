using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class HomingRocket : ProjectileAttack 
{

	public GameObject explosionRep;
	ObjectPool explosionPool;
	public float explosionSize;
	public float explosionDuration;
	public float passiveProcChance;
	public LayerMask onDestroyTargets;

	void Start()
	{

		if(explosionPool == null && explosionRep != null) {
			explosionPool = PoolManager.Instance.GetPoolByRepresentative(explosionRep);
		}
		base.Start ();
	}

	override protected void fireProjectile(GameObject bullet, Transform player)
	{
		if(channeler == null) {
			channeler = player.FindChild("channeler");
		}
		bullet.transform.position = channeler.position;
		bullet.transform.right = player.up;//*Mathf.Sign (player.localScale.x);
		PoolableProjectile proj = bullet.GetComponent<PoolableProjectile>();
		proj.SetOnDestroy(createExplosion, onDestroyTargets);
		proj.SetOnCollision(collisionExplosion, onCollisionTargets);
		//bullet.SendMessage("SetOnDestroy",new UpgradeAction(createExplosion,onDestroyTargets));
		//bullet.SendMessage("SetOnCollision",new UpgradeAction(createExplosion,onCollisionTargets));

	}

	void createExplosion(Transform projectile)
	{
		if(explosionPool != null) {
			GameObject explosion = explosionPool.getPooled();
			explosion.SetActive (true);
			explosion.transform.position = projectile.position;
			explosion.transform.rotation = projectile.rotation;
			Vector3 scale = explosion.transform.localScale;
			scale = explosionSize*scale;
			explosion.transform.localScale = scale;
			explosion.SendMessage("SetDuration",explosionDuration);
			explosion.SendMessage("SetBlastEffect",new UpgradeAction(onCollision,onCollisionTargets));
			explosion.SendMessage("StartDelay",0f);
		}
	}

	void collisionExplosion(Transform projectile, Transform target)
	{
		createExplosion(projectile);
	}

	override protected void defaultCollision(Transform projectile, Transform target)
	{
		if(target.GetComponent<GameActor>())
			target.SendMessage("Damage",effectSize);
		if(target.rigidbody2D != null) {
			Vector3 forceDir = target.position - projectile.position;
			Vector3 force = forceDir.normalized*effectSize*10;
			target.rigidbody2D.AddForce(force);
		}
	}

	public override void applyPassive (Transform player)
	{
		PeriodicAction action = player.gameObject.AddComponent<PeriodicAction>();
		passiveActionID = action.GetInstanceID();
		action.init(activeEffect,cooldown,passiveProcChance);
	}

	public override void undoPassive (Transform player)
	{
		PeriodicAction[] actions = player.gameObject.GetComponents<PeriodicAction>();
		PeriodicAction action = null;
		foreach(PeriodicAction a in actions)
			if(a.GetInstanceID() == passiveActionID)
				action = a;
		if(action)
			Destroy(action);
	}

	

}
