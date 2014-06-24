using UnityEngine;
using System.Collections;

public class DOTBlast : CloseBlast
{
	
	public int intervals;
	public LayerMask onHitPassiveTargets;
	int passiveActionID = 0;

	override public void burstEffect(Transform blast, Transform target)
	{
		if(target.GetComponent<GameActor>()) {
			target.SendMessage("ApplyDOT",new Vector2(effectSize,intervals));
			target.SendMessage("SetColor",Color.red);
		}
	}

	override public void applyPassive(Transform player)
	{
		ActionOnHit action = player.gameObject.AddComponent<ActionOnHit>();
		action.init(burstEffect,onHitPassiveTargets,cooldown);
		passiveActionID = action.GetInstanceID();
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

