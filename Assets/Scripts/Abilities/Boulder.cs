using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Boulder : ProjectileAttack 
{

	public GameObject clusterObj;
	ObjectPool clusterPool;
	public int clusterSize;
	public float clusterForce;


	void Start()
	{
		base.Start();
		if(clusterPool == null && clusterObj != null) {
			clusterPool = ObjectPool.GetPoolByRepresentative(clusterObj);
		}
	}

	override protected void fireProjectile(GameObject bullet, Transform player)
	{
		if(channeler == null) {
			channeler = player.FindChild("channeler");
		}
		bullet.transform.position = channeler.position;
		bullet.SendMessage("SetOnDestroy",new UpgradeAction(spawnClusters));
		float angle = 45*Mathf.Sign (player.localScale.x);
		Vector3 facingDir = player.right*Mathf.Sign (player.localScale.x);
		Vector3 firingDir = Quaternion.AngleAxis(angle,Vector3.forward)*facingDir;
		bullet.rigidbody2D.AddForce(firingDir*bulletVelocity);
	}

	void spawnClusters(Transform projectile)
	{
		if(clusterPool != null) {
			for(int i = 0; i < clusterSize; i++) {
				GameObject proj = clusterPool.getPooled();
				proj.SetActive(true);
				proj.SendMessage("SetOnCollision",new UpgradeAction(onCollision,onCollisionTargets));
				proj.SendMessage("SetOnDestroy",new UpgradeAction(onDestroy));
				proj.transform.position = projectile.position;
				Vector2 force = UnityEngine.Random.insideUnitCircle*clusterForce;
				proj.rigidbody2D.AddForce(force);
			}
		}
	}

	override protected void reset()
	{
		onDestroy = spawnClusters;
	}

	override protected void upgradeOtherAbility(Ability other)
	{
		if(other.GetType().BaseType != typeof(Ability)) {
			if(other.GetType().BaseType == typeof(ProjectileAttack)) {
				ProjectileAttack pa = (ProjectileAttack)other;
				pa.onDestroy = spawnClusters;
			}
		}
	}
	
	override public void passiveEffect(Transform player)
	{
		
	}
	
}
