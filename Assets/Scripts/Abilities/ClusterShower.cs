using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ClusterShower : Special 
{

	public GameObject clusterObj;
	ObjectPool clusterPool;
	public float fireForce;
	public int fireAmount = 5;
	float maxDeviation = 5;
	protected Transform channeler;

	public Action<Transform, Transform> onCollision;
	public LayerMask onCollisionTargets;


	void Start()
	{
		if(upgrade != null) {
			upgradeAbility(upgrade);
		}
		onCollision = defaultCollision;
		if(clusterPool == null && clusterObj != null) {
			clusterPool = ObjectPool.GetPoolByRepresentative(clusterObj);
		}
	}

	override public void activeEffect(Transform player)
	{
		if(channeler == null) {
			channeler = player.FindChild("channeler");
		}


		for(int i = 0; i < fireAmount; i++) {
			StartCoroutine(Timers.Countdown<Transform>(UnityEngine.Random.Range (0f,0.1f),shootCluster,player));
		}
	}

	void shootCluster(Transform player)
	{
		Vector3 facingDir = player.right*Mathf.Sign (player.localScale.x);
		GameObject bullet = clusterPool.getPooled();
		bullet.SetActive(true);
		bullet.SendMessage("IgnoreCollider",player.collider2D);
		bullet.transform.position = channeler.position;
		bullet.SendMessage("SetOnCollision",new UpgradeAction(onCollision,onCollisionTargets));
		float angle = (45 + UnityEngine.Random.Range (-maxDeviation,maxDeviation))*Mathf.Sign (player.localScale.x);
		Vector3 firingDir = Quaternion.AngleAxis(angle,Vector3.forward)*facingDir;
		bullet.rigidbody2D.AddForce(firingDir*fireForce);
	}

	public void defaultCollision(Transform projectile, Transform target)
	{
		target.SendMessage("Damage",effectSize);
	}



	override protected void reset()
	{

	}

	override protected void upgradeOtherAbility(Ability other)
	{
		if(other.GetType().BaseType == typeof(ProjectileAttack)) {
			ProjectileAttack pa = (ProjectileAttack)other;

		} else if(other.GetType().BaseType == typeof(CloseBlast)) {
			CloseBlast cb = (CloseBlast)other;

		} else if(other.GetType().BaseType == typeof(Buff)) {

		} else if(other.GetType().BaseType == typeof(Special)) {
			//individual ifs for each ab
		}
	}
	
	override public void passiveEffect(Transform player)
	{
		
	}
	
}
