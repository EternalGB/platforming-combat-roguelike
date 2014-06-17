using UnityEngine;
using System.Collections;
using System;

public abstract class CloseBlast : Ability
{

	public GameObject blastObj;
	ObjectPool blastPool;
	public Action<Transform, Transform> onHitByBurst;
	public Action<Transform, Transform> blastFunc;
	public float blastDelay;
	public LayerMask burstTargets;
	protected Transform channeler;

	void Start()
	{
		base.Start();
		if(blastObj != null) {
			blastPool = ObjectPool.GetPoolByRepresentative(blastObj);
		}
		onHitByBurst = burstEffect;
		blastFunc = createBlast;
	}

	public abstract void burstEffect(Transform blast, Transform target);

	override public void activeEffect(Transform player)
	{
		if(channeler == null) {
			channeler = player.FindChild("channeler");
		}
		blastFunc(channeler,player);
	}

	void createBlast(Transform location, Transform player)
	{
		GameObject blast = blastPool.getPooled();
		blast.SetActive(true);
		blast.SendMessage ("StartDelay",blastDelay);
		blast.SendMessage("SetBlastEffect",new UpgradeAction(onHitByBurst,burstTargets));
		blast.transform.position = location.position;
		blast.transform.right = location.right*Mathf.Sign (location.localScale.x);
	}
	
	override protected void upgradeOtherAbility(Ability other)
	{
		print(abilityName + " upgrading " + other.abilityName);
		if(other.GetType().BaseType == typeof(ProjectileAttack)) {
			ProjectileAttack pa = (ProjectileAttack)other;
			pa.onCollision = createBlast;
			pa.onCollisionTargets = burstTargets;
		} else if(other.GetType().BaseType == typeof(CloseBlast)) {
			CloseBlast cb = (CloseBlast)other;
			cb.onHitByBurst = onHitByBurst;
			cb.burstTargets = burstTargets;
		} else if(other.GetType().BaseType == typeof(Buff)) {
			Buff b = (Buff)other;
			onHitByBurst = b.buffEffect;
			b.activeFunc = activeEffect;
		} else if(other.GetType().BaseType == typeof(Special)) {
			if(other.GetType() == typeof(Dash)) {
				
			} else if(other.GetType() == typeof(Glide)) {
				
			} else if(other.GetType() == typeof(ClusterShower)) {
				ClusterShower cs = (ClusterShower)other;
				cs.onCollision = createBlast;
				cs.onCollisionTargets = burstTargets;
			}
		}
	}

	void upgradeProjectileAttack(ProjectileAttack pa)
	{
		pa.onCollision = burstEffect;
	}

	override protected void reset()
	{
		
	}

	override public void passiveEffect(Transform player)
	{

	}

}

