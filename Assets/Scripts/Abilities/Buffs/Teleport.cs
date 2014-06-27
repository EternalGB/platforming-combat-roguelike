using UnityEngine;
using System.Collections;

public class Teleport : Buff
{

	public LayerMask passiveTriggers;
	public float passiveChance;
	int passiveActionID;

	public override void buffEffect (Transform applier, Transform target)
	{
		GameActor actor;
		if(actor = target.GetComponent<GameActor>()) {
			Vector3 translation = (Vector3)(actor.facingDir*effectSize);
			if(!Physics2D.OverlapPoint(target.position + translation,actor.groundLayer))
				target.position += translation;
		}
	}

	public override void undoBuff (Transform applier, Transform target)
	{
		//do nothing
	}

	//just swaps the args
	void passiveAction(Transform t1, Transform t2)
	{
		buffEffect(t2,t1);
	}

	public override void applyPassive (Transform player)
	{
		ChanceOnHit script = player.gameObject.AddComponent<ChanceOnHit>();
		script.init(passiveAction, passiveTriggers, passiveChance);
		passiveActionID = script.GetInstanceID();
	}

	public override void undoPassive (Transform player)
	{
		ChanceOnHit[] actions = player.gameObject.GetComponents<ChanceOnHit>();
		ChanceOnHit action = null;
		foreach(ChanceOnHit a in actions)
			if(a.GetInstanceID() == passiveActionID)
				action = a;
		if(action)
			Destroy(action);
	}



}

