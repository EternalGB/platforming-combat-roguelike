using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GrenadeToss : ProjectileAttack 
{

	public GameObject explosionRep;
	ObjectPool explosionPool;
	public float explosionSize;
	public float explosionDuration;
	public LayerMask onDestroyTargets;

	void Start()
	{

		if(explosionPool == null && explosionRep != null) {
			explosionPool = ObjectPool.GetPoolByRepresentative(explosionRep);
		}
		base.Start ();
	}

	override protected void fireProjectile(GameObject bullet, Transform player)
	{
		if(channeler == null) {
			channeler = player.FindChild("channeler");
		}
		bullet.transform.position = channeler.position;
		
		float angle = 45*Mathf.Sign (player.localScale.x);
		Vector3 facingDir = player.right*Mathf.Sign (player.localScale.x);
		Vector3 firingDir = Quaternion.AngleAxis(angle,Vector3.forward)*facingDir;
		bullet.rigidbody2D.AddForce(firingDir*bulletVelocity);
		bullet.SendMessage("SetOnDestroy",new UpgradeAction(createExplosion,onDestroyTargets));
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
	

	

}
