using UnityEngine;
using System.Collections;
using System;

public abstract class ProjectileAttack : Ability
{

	protected ObjectPool projectiles;
	public GameObject projectileRep;
	public GameObject ProjectileRep
	{
		get
		{
			return projectileRep;
		}
		set
		{
			projectileRep = value;
			projectiles = ObjectPool.GetPoolByRepresentative(projectileRep);
		}

	}
	protected Transform channeler;
	public float bulletVelocity = 1000;
	public Action<Transform, Transform> onCollision;
	public LayerMask onCollisionTargets;

	private GameObject origProjectileRep;
	private float origBulletVel;
	private LayerMask origCollisionTargets;

	protected int passiveActionID = 0;

	public void Start()
	{
		base.Start();
		onCollision = defaultCollision;
		origProjectileRep = projectileRep;
		origBulletVel = bulletVelocity;
		origCollisionTargets = onCollisionTargets;

		projectiles = ObjectPool.GetPoolByRepresentative(ProjectileRep);
	}


	override sealed public void activeEffect(Transform player)
	{
		GameObject bullet = projectiles.getPooled();
		bullet.SetActive(true);
		bullet.SendMessage("IgnoreCollider",PlayerController.GlobalPlayerInstance.collider2D);
		fireProjectile(bullet,player);
	}

	protected abstract void fireProjectile(GameObject bullet, Transform player); 

	override protected void upgradeOtherAbility(Ability other)
	{
		print(abilityName + " upgrading " + other.abilityName);
		if(other.GetType().BaseType == typeof(ProjectileAttack)) {
			ProjectileAttack pa = (ProjectileAttack)other;
			pa.onCollision = onCollision;
			pa.onCollisionTargets = onCollisionTargets;
		} else if(other.GetType().BaseType == typeof(CloseBlast)) {
			CloseBlast cb = (CloseBlast)other;
			cb.onHitByBurst = onCollision;
			cb.burstTargets = onCollisionTargets;
		} else if(other.GetType().BaseType == typeof(Buff)) {
			Buff b = (Buff)other;
			onCollision = b.buffEffect;
			b.activeFunc = activeEffect;
		} else if(other.GetType().BaseType == typeof(Special)) {
			if(other.GetType() == typeof(Dash)) {
				Dash d = (Dash)other;
				d.preDashAction = activeEffect;
			} else if(other.GetType() == typeof(ClusterShower)) {
				ClusterShower cs = (ClusterShower)other;
				cs.onCollision = onCollision;
				cs.onCollisionTargets = onCollisionTargets;
			}  else if(other.GetType() == typeof(OrbGenerator)) {
				OrbGenerator og = (OrbGenerator)other;
				og.onCollision = onCollision;
				og.onCollisionTargets = onCollisionTargets;
			}
		}
	}

	protected virtual void defaultCollision(Transform projectile, Transform target)
	{
		if(target.GetComponent<GameActor>())
			target.SendMessage("Damage",effectSize);
	}

	override protected void reset()
	{
		ProjectileRep = origProjectileRep;
		bulletVelocity = origBulletVel;
		onCollision = defaultCollision;
		onCollisionTargets = origCollisionTargets;
	}

	override public void applyPassive(Transform player)
	{
		ActionOnHit action = player.gameObject.AddComponent<ActionOnHit>();
		action.init(defaultCollision,onCollisionTargets,cooldown);
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

