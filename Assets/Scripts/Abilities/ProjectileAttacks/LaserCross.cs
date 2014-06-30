using UnityEngine;
using System.Collections;

public class LaserCross : ProjectileAttack
{

	public float velocityLoss;

	protected override void fireProjectile (GameObject bullet, Transform player)
	{
		StraightBoomerang sb = bullet.GetComponent<StraightBoomerang>();
		if(sb) {
			if(channeler == null) {
				channeler = player.FindChild("channeler");
			}
			bullet.SendMessage("SetOnCollision",new UpgradeAction(onCollision,onCollisionTargets));
			bullet.transform.position = channeler.position;
			bullet.transform.right = player.right*Mathf.Sign (player.localScale.x);
			bullet.rigidbody2D.AddForce(player.right*Mathf.Sign(player.localScale.x)*bulletVelocity);
			sb.origVelocity = Mathf.Sign(player.localScale.x)*bulletVelocity;
			sb.velocityLoss = velocityLoss;
		}
	}
		
}

