using UnityEngine;
using System.Collections;
using System;

public abstract class CloseBlast : Ability
{

	public GameObject blastObj;
	ObjectPool blastPool;
	Action<Transform, Transform> onHitByBurst;
	public Vector2 fireLocation;
	public float blastDelay;
	public LayerMask burstTargets;
	protected Transform channeler;

	void Start()
	{
		if(blastObj != null) {
			blastPool = ObjectPool.GetPoolByRepresentative(blastObj);
		}
		onHitByBurst = burstEffect;
	}

	public abstract void burstEffect(Transform blast, Transform target);

	override public void activeEffect(Transform player)
	{
		if(channeler == null) {
			channeler = player.FindChild("channeler");
		}
		GameObject blast = blastPool.getPooled();
		blast.SetActive(true);
		blast.SendMessage ("StartDelay",blastDelay);
		blast.SendMessage("SetBlastEffect",new UpgradeAction(onHitByBurst,burstTargets));
		blast.transform.position = 
			channeler.position + new Vector3(fireLocation.x*Mathf.Sign(player.localScale.x),fireLocation.y,0);
		blast.transform.rotation = channeler.rotation;

	}
	
	override protected void upgradeOtherAbility(Ability other)
	{
		//if the ability is not a direct child of the ability class
		//then is may have some other base type that we have to check
		if(other.GetType().BaseType != typeof(Ability)) {
			if(other.GetType().BaseType == typeof(ProjectileAttack)) {
				upgradeProjectileAttack((ProjectileAttack)other);
			}
			//else it's an instance of a direct child class
		} else if(other.GetType () == typeof(ProjectileAttack)) {
			upgradeProjectileAttack((ProjectileAttack)other);
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

