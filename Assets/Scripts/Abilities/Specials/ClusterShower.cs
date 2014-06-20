using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ClusterShower : Special
{

	public GameObject clusterObj;
	public GameObject ClusterObj
	{
		get
		{
			return clusterObj;
		}
		set
		{
			clusterObj = value;
			clusterPool = ObjectPool.GetPoolByRepresentative(clusterObj);
		}
	}
	ObjectPool clusterPool;
	public float fireForce;
	public int fireAmount = 5;
	float maxDeviation = 5;
	protected Transform channeler;

	public Action<Transform, Transform> onCollision;
	public LayerMask onCollisionTargets;

	private GameObject origClusterObj;
	private float origFireForce;
	private int origFireAmount;
	private LayerMask origOnCollisionTargets;

	void Start()
	{
		onCollision = defaultCollision;
		if(clusterPool == null && clusterObj != null) {
			clusterPool = ObjectPool.GetPoolByRepresentative(clusterObj);
		}

		origClusterObj = clusterObj;
		origFireForce = fireForce;
		origFireAmount = fireAmount;
		origOnCollisionTargets = onCollisionTargets;

		base.Start();
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
		bullet.SendMessage("IgnoreCollider",PlayerController.GlobalPlayerInstance.collider2D);
		bullet.transform.position = channeler.position;
		bullet.SendMessage("SetOnCollision",new UpgradeAction(onCollision,onCollisionTargets));
		float angle = (45 + UnityEngine.Random.Range (-maxDeviation,maxDeviation))*Mathf.Sign (player.localScale.x);
		Vector3 firingDir = Quaternion.AngleAxis(angle,Vector3.forward)*facingDir;
		bullet.rigidbody2D.AddForce(firingDir*fireForce);
	}

	//shoots in an angle around from Vector2.right
	void shootCluster(Transform origin, float baseAngle)
	{
		float angle = baseAngle + UnityEngine.Random.Range (-maxDeviation,maxDeviation);

		GameObject bullet = clusterPool.getPooled();
		bullet.SetActive(true);
		bullet.transform.position = Quaternion.AngleAxis(angle,Vector3.forward)*origin.position;
		bullet.SendMessage("IgnoreCollider",PlayerController.GlobalPlayerInstance.collider2D);
		bullet.SendMessage("SetOnCollision",new UpgradeAction(onCollision,onCollisionTargets));

		Vector3 firingDir = Quaternion.AngleAxis(angle,Vector3.forward)*Vector2.right;
		bullet.rigidbody2D.AddForce(firingDir*fireForce);
	}

	void sprayClusters(Transform location, Transform notNeeded)
	{
		if(location.gameObject.layer != LayerMask.NameToLayer("Ground")) {
			for(int i = 0; i < fireAmount; i++) {
				StartCoroutine(Timers.Countdown<Transform, float>(UnityEngine.Random.Range (0f,0.1f),shootCluster,location,90));
			}
		}
	}

	void sprayClusters(Transform location)
	{
		sprayClusters(location,null);
	}
	
	public void defaultCollision(Transform projectile, Transform target)
	{
		if(target.GetComponent<GameActor>())
			target.SendMessage("Damage",effectSize);
	}



	override protected void reset()
	{
		onCollision = defaultCollision;
		ClusterObj = origClusterObj;
		fireForce = origFireForce;
		fireAmount = origFireAmount;
		onCollisionTargets = origOnCollisionTargets;
	}

	override protected void upgradeOtherAbility(Ability other)
	{
		if(other.GetType().BaseType == typeof(ProjectileAttack)) {
			ProjectileAttack pa = (ProjectileAttack)other;
			pa.onCollision = sprayClusters;
		} else if(other.GetType().BaseType == typeof(CloseBlast)) {
			CloseBlast cb = (CloseBlast)other;
			cb.onHitByBurst = sprayClusters;
		} else if(other.GetType().BaseType == typeof(Buff)) {
			Buff b = (Buff)other;
			onCollision = b.buffEffect;
			b.activeFunc = activeEffect;
		} else if(other.GetType().BaseType == typeof(Special)) {
			if(other.GetType() == typeof(Dash)) {
				Dash d = (Dash)other;
				d.preDashAction = sprayClusters;
			} 
		}
	}
	
	override public void passiveEffect(Transform player)
	{
		
	}
	
}
