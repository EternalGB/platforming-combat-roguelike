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
			clusterPool = PoolManager.Instance.GetPoolByRepresentative(clusterObj);
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

	public LayerMask passiveHitTargets;
	int passiveActionID = 0;
	bool canSpray = true;

	void Start()
	{
		onCollision = defaultCollision;
		if(clusterPool == null && clusterObj != null) {
			clusterPool = PoolManager.Instance.GetPoolByRepresentative(clusterObj);
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

	GameObject getBullet(Transform spawnLoc)
	{
		GameObject bullet = clusterPool.getPooled();
		bullet.SetActive(true);
		bullet.transform.position = spawnLoc.position;
		
		PoolableProjectile proj = bullet.GetComponent<PoolableProjectile>();
		proj.IgnoreCollider(PlayerController.GlobalPlayerInstance.collider2D);
		proj.SetOnCollision(onCollision,onCollisionTargets);

		proj.collider2D.enabled = false;
		StartCoroutine(Timers.Countdown<GameObject>(0.1f,enableBulletCollision,bullet));

		return bullet;
	}

	void enableBulletCollision(GameObject bullet)
	{
		bullet.collider2D.enabled = true;
	}

	void shootCluster(Transform player)
	{
		Vector3 facingDir = player.right*Mathf.Sign (player.localScale.x);
		GameObject bullet = getBullet(channeler);

		float angle = (45 + UnityEngine.Random.Range (-maxDeviation,maxDeviation))*Mathf.Sign (player.localScale.x);
		Vector3 firingDir = Quaternion.AngleAxis(angle,Vector3.forward)*facingDir;
		bullet.rigidbody2D.AddForce(firingDir*fireForce);
	}

	//shoots in an angle around from Vector2.right
	void shootCluster(Transform origin, float baseAngle)
	{
		float angle = baseAngle + UnityEngine.Random.Range (-maxDeviation,maxDeviation);
		GameObject bullet = getBullet(origin);
		Vector3 firingDir = Quaternion.AngleAxis(angle,Vector3.forward)*Vector2.right;
		bullet.rigidbody2D.AddForce(firingDir*fireForce);
	}

	List<Transform> lastSprayLocs;

	void sprayClusters(Transform location, Transform notNeeded)
	{
		if(lastSprayLocs == null)
			lastSprayLocs = new List<Transform>();
		if(!lastSprayLocs.Contains(location)) {
			canSpray = true;
		}
		lastSprayLocs.Add(location);
		//canSpray puts a global cap on how fast this can fire
		if(canSpray && location.gameObject.layer != LayerMask.NameToLayer("Ground")) {
			//print("Spraying clusters from " + location + " at " + location.position);
			for(int i = 0; i < fireAmount; i++) {
				StartCoroutine(Timers.Countdown<Transform, float>(UnityEngine.Random.Range (0f,0.1f),shootCluster,location,90));
			}
			canSpray = false;
			Invoke("enableSpray",cooldown);
		}
	}

	void passiveSpray(Transform location, Transform other)
	{
		sprayClusters(location);
	}

	void upgradeSpray(Transform other, Transform location)
	{
		sprayClusters(location);
	}

	void enableSpray()
	{
		canSpray = true;
		if(lastSprayLocs != null)
			lastSprayLocs.Clear();
		else
			lastSprayLocs = new List<Transform>();
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
			pa.onCollision = upgradeSpray;
		} else if(other.GetType().BaseType == typeof(CloseBlast)) {
			CloseBlast cb = (CloseBlast)other;
			cb.onHitByBurst = upgradeSpray;
		} else if(other.GetType().BaseType == typeof(Buff)) {
			Buff b = (Buff)other;
			onCollision = b.buffEffect;
			b.activeFunc = activeEffect;
		} else if(other.GetType().BaseType == typeof(Special)) {
			if(other.GetType() == typeof(Dash)) {
				Dash d = (Dash)other;
				d.preDashAction = sprayClusters;
			}  else if(other.GetType() == typeof(OrbGenerator)) {
				OrbGenerator og = (OrbGenerator)other;
				og.onCollision = upgradeSpray;
			}
		}
	}
	
	override public void applyPassive(Transform player)
	{
		ActionOnHit script = player.gameObject.AddComponent<ActionOnHit>();
		script.init(passiveSpray, passiveHitTargets, cooldown);
		passiveActionID = script.GetInstanceID();
	}
	
	override public void undoPassive(Transform player)
	{
		ActionOnHit[] actions = player.gameObject.GetComponents<ActionOnHit>();
		ActionOnHit action = null;
		foreach(ActionOnHit a in actions)
			if(a.GetInstanceID() == passiveActionID)
				action = a;
		if(action)
			Destroy(action);
	}
	
}
