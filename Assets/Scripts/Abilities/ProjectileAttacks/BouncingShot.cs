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
		PoolableProjectile proj = bullet.GetComponent<PoolableProjectile>();
		proj.SetOnCollision(onCollision, onCollisionTargets);
		bullet.GetComponent<BouncingProjectile>().numBounces = numBounces;
	}




}
