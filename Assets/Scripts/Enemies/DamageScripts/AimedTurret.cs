using UnityEngine;
using System.Collections;

public class AimedTurret : MonoBehaviour
{

	public LayerMask targets;
	public Transform firePosition;
	public GameObject projectileRep;
	ObjectPool projectiles;
	public float damage;
	public float fireInterval;
	public float projectileVelocity;
	public float detectionRadius;
	Transform target;

	void Start()
	{
		projectiles = ObjectPool.GetPoolByRepresentative(projectileRep);
		InvokeRepeating("Fire",0,fireInterval);
	}

	void Update()
	{
		target = getTarget();
	}

	Transform getTarget()
	{
		Collider2D detected = Physics2D.OverlapCircle(transform.position,detectionRadius,targets);
		if(detected != null)
			return detected.transform;
		else
			return null;
	}

	void Fire()
	{
		if(target != null) {
			GameObject bullet = (GameObject)projectiles.getPooled();
			bullet.SetActive(true);
			bullet.SendMessage ("IgnoreCollider",collider2D);
			bullet.SendMessage("SetOnCollision",new UpgradeAction(ProjectileDamage,targets));
			bullet.transform.position = firePosition.position;
			bullet.rigidbody2D.AddForce((target.position - firePosition.position).normalized*projectileVelocity);
		}
	}

	void ProjectileDamage(Transform projectile, Transform target)
	{
		target.SendMessage("Damage",damage);
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position,detectionRadius);
	}

}

