using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LobShot : ProjectileAttack 
{



	override protected void fireProjectile(GameObject bullet, Transform player)
	{
		if(channeler == null) {
			channeler = player.FindChild("channeler");
		}
		PoolableProjectile proj = bullet.GetComponent<PoolableProjectile>();
		proj.SetOnCollision(onCollision, onCollisionTargets);
		bullet.transform.position = channeler.position;
		
		float angle = 45*Mathf.Sign (player.localScale.x);
		Vector3 facingDir = player.right*Mathf.Sign (player.localScale.x);
		Vector3 firingDir = Quaternion.AngleAxis(angle,Vector3.forward)*facingDir;
		bullet.rigidbody2D.AddForce(firingDir*bulletVelocity);
	}
	
	

}
