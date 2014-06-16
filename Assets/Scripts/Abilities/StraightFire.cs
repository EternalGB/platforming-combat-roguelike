using UnityEngine;
using System.Collections;

public class StraightFire : ProjectileAttack
{

	void Start()
	{
		onCollision = defaultCollision;
		base.Start();
	}

	override protected void fireProjectile(GameObject bullet, Transform player)
	{
		if(channeler == null) {
			channeler = player.FindChild("channeler");
		}
		;
		bullet.SendMessage("SetOnCollision",new UpgradeAction(onCollision,onCollisionTargets));
		bullet.transform.position = channeler.position;
		bullet.transform.right = player.right*Mathf.Sign (player.localScale.x);
		bullet.rigidbody2D.AddForce(player.right*Mathf.Sign(player.localScale.x)*bulletVelocity);
	}
	
	public void defaultCollision(Transform projectile, Transform target)
	{
		target.SendMessage("Damage",effectSize);
	}
}

