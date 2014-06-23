using UnityEngine;
using System.Collections.Generic;

public class GravityReduce : Buff
{

	Dictionary<Transform, float> originalGravs;

	void Start()
	{
		originalGravs = new Dictionary<Transform, float>();
		base.Start();
	}

	override public void buffEffect(Transform applier, Transform target)
	{
		GameActor actor;
		if(actor = target.GetComponent<GameActor>()) {
			originalGravs.Add(target,actor.rigidbody2D.gravityScale);
			actor.rigidbody2D.gravityScale -= actor.rigidbody2D.gravityScale*effectSize;
		}
	}
	
	override public void undoBuff(Transform applier, Transform target)
	{
		GameActor actor;
		if(actor = target.GetComponent<GameActor>()) {
			actor.rigidbody2D.gravityScale = originalGravs[target];
			originalGravs.Remove(target);
		}
	}
	

	override public void applyPassive(Transform player)
	{
		GameActor actor = player.GetComponent<GameActor>();
		actor.rigidbody2D.gravityScale -= effectSize/2;
	}
	
	override public void undoPassive(Transform player)
	{
		GameActor actor = player.GetComponent<GameActor>();
		actor.rigidbody2D.gravityScale += effectSize/2;
	}
}

