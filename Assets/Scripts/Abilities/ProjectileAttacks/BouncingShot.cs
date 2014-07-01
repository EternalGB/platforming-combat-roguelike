using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BouncingShot : ProjectileAttack 
{

	public int numBounces = 3;

	override protected void fireProjectile(GameObject bullet, Transform player)
	{
		if(channeler == null) {
			channeler = player.FindChild("channeler");
		}
		bullet.transform.position = channeler.position;
		bullet.transform.right = player.right*Mathf.Sign (player.localScale.x);
		bullet.SendMessage("SetOnDestroy",new UpgradeAction(onCollision,onCollisionTargets));
		bullet.SendMessage("SetOnCollision",new UpgradeAction(onCollision,onCollisionTargets));
		bullet.GetComponent<BouncingProjectile>().numBounces = numBounces;
	}




}
