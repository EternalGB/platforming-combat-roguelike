using UnityEngine;
using System.Collections;

public class GravitySet : Buff
{

	override public void buffEffect(Transform applier, Transform target)
	{
		GameActor actor;
		if(actor = target.GetComponent<GameActor>())
			actor.rigidbody2D.gravityScale = effectSize;
	}
	
	override public void undoBuff(Transform applier, Transform target)
	{
		GameActor actor;
		if(actor = target.GetComponent<GameActor>())
			actor.rigidbody2D.gravityScale = 3;
	}
	
	
	
	override protected void reset()
	{
		
	}

	override public void passiveEffect(Transform player)
	{
		
	}
}

