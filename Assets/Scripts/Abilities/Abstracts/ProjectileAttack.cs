using UnityEngine;
using System.Collections;
using System;

public abstract class ProjectileAttack : Ability
{

	protected ObjectPool projectiles;
	public GameObject projectileRep;
	protected Transform channeler;
	public float bulletVelocity = 1000;
	public Action<Transform, Transform> onCollision;
	public LayerMask onCollisionTargets;
	
	public void Start()
	{
		projectiles = ObjectPool.GetPoolByRepresentative(projectileRep);
		if(upgrade != null) {
			upgradeAbility(upgrade);
		}
	}


	override sealed public void activeEffect(Transform player)
	{
		GameObject bullet = projectiles.getPooled();
		bullet.SetActive(true);
		bullet.SendMessage("IgnoreCollider",player.collider2D);
		fireProjectile(bullet,player);
	}

	protected abstract void fireProjectile(GameObject bullet, Transform player); 

	override protected void upgradeOtherAbility(Ability other)
	{
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

		} else if(other.GetType().BaseType == typeof(Special)) {
			if(other.GetType() == typeof(Dash)) {

			} else if(other.GetType() == typeof(Glide)) {

			} else if(other.GetType() == typeof(ClusterShower)) {
				ClusterShower cs = (ClusterShower)other;
				cs.onCollision = onCollision;
				cs.onCollisionTargets = onCollisionTargets;
			}
		}
	}

	override protected void reset()
	{

	}

	override public void passiveEffect(Transform player)
	{
		
	}

}

