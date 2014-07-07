using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AlienTracer : ProjectileAttack 
{

	public float projManeuverability;

	override protected void fireProjectile(GameObject bullet, Transform player)
	{
		if(channeler == null) {
			channeler = player.FindChild("channeler");
		}
		TrackingProjectile proj = bullet.GetComponent<TrackingProjectile>();
		proj.SetOnCollision(onCollision, onCollisionTargets);
		proj.maxTurn = projManeuverability;
		bullet.transform.position = channeler.position;
		
		float angle = 45*Mathf.Sign (player.localScale.x);
		Vector3 facingDir = player.right*Mathf.Sign (player.localScale.x);
		Vector3 firingDir = Quaternion.AngleAxis(angle,Vector3.forward)*facingDir;
		bullet.transform.right = firingDir;
		//bullet.rigidbody2D.AddForce(firingDir*bulletVelocity);
	}
	
	

}
