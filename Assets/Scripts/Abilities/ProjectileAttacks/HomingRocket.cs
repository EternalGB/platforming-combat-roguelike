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


	void Start()
	{
		base.Start ();
		if(explosionPool == null && explosionRep != null) {
			explosionPool = ObjectPool.GetPoolByRepresentative(explosionRep);
		}
		onCollision = defaultBlast;
	}

	override protected void fireProjectile(GameObject bullet, Transform player)
	{
		if(channeler == null) {
			channeler = player.FindChild("channeler");
		}
		bullet.transform.position = channeler.position;
		bullet.transform.right = player.right*Mathf.Sign (player.localScale.x);
		bullet.SendMessage("SetOnDestroy",new UpgradeAction(onCollision,onCollisionTargets));
		bullet.SendMessage("SetOnCollision",new UpgradeAction(onCollision,onCollisionTargets));

	}

	public void createExplosion(Transform player, Transform projectile)
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
			explosion.SendMessage("SetBlastEffect",new UpgradeAction(defaultBlast,onCollisionTargets));
			explosion.SendMessage("StartDelay",0f);
		}
	}

	public void defaultBlast(Transform projectile, Transform target)
	{
		if(target.GetComponent<GameActor>())
			target.SendMessage("Damage",effectSize);
		if(target.rigidbody2D != null) {
			Vector3 forceDir = target.position - projectile.position;
			Vector3 force = forceDir.normalized*effectSize*10;
			target.rigidbody2D.AddForce(force);
		}
	}


	
	override public void passiveEffect(Transform player)
	{

	}

}
