using UnityEngine;
using System.Collections;


public class ProjectileShield : Buff
{

	public GameObject shieldPrefab;
	bool shieldExists = false;
	bool passiveOn = false;
	GameObject currentShield;

	void shieldDestroyed(ProjectileBlocker blocker)
	{
		//print("shieldDestroyed");
		shieldExists = false;
		if(passiveOn) {
			Invoke("passiveDelegate",cooldown*2);
		}
	}

	public override void buffEffect (Transform applier, Transform target)
	{
		if(effectSize > 0) {
			addShield(target);
		} else {
			//multiply damage
			GameActor actor;
			if(actor = target.GetComponent<GameActor>())
				actor.SetDamageTakenMultiplier(Mathf.Clamp(-effectSize/2,2,10));
		}
	}

	public override void undoBuff (Transform applier, Transform target)
	{
		if(effectSize > 0) {
			if(currentShield)
				Destroy(currentShield);
		} else {
			GameActor actor;
			if(actor = target.GetComponent<GameActor>())
				actor.SetDamageTakenMultiplier(1);
		}
	}

	void addShield(Transform target)
	{

		if(!shieldExists) {
			GameObject shield = (GameObject)Instantiate(shieldPrefab,target.position,Quaternion.identity);
			Vector3 extents = target.collider2D.bounds.extents;
			float radius = Vector3.Magnitude(extents);
			shield.transform.localScale = new Vector3(radius*2f,radius*2f,1);
			ProjectileBlocker script = shield.GetComponent<ProjectileBlocker>();
			script.numCanBlock = (int)effectSize;
			script.destroyEvent += shieldDestroyed;
			shieldExists = true;
			shield.transform.parent = target;
			currentShield = shield;
		}
	}

	void passiveDelegate()
	{
		addShield(PlayerController.GlobalPlayerInstance.transform);
	}

	public override void applyPassive (Transform player)
	{
		if(!shieldExists) {
			if(currentShield != null)
				Destroy(currentShield);
			addShield(player);
		}
		passiveOn = true;
	}

	public override void undoPassive (Transform player)
	{
		passiveOn = false;
		if(shieldExists) {
			Destroy(currentShield);
			shieldExists = false;
		}
	}

}

