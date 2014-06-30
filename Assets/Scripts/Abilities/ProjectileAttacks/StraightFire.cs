using UnityEngine;
using System.Collections;

public class StraightFire : ProjectileAttack
{



	override protected void fireProjectile(GameObject bullet, Transform player)
	{
		if(channeler == null) {
			channeler = player.FindChild("channeler");
		}
		bullet.SendMessage("SetOnCollision",new UpgradeAction(onCollision,onCollisionTargets));
		bullet.transform.position = channeler.position;
		bullet.transform.right = player.right*Mathf.Sign (player.localScale.x);
		bullet.rigidbody2D.AddForce(player.right*Mathf.Sign(player.localScale.x)*bulletVelocity);
	}
	

}

