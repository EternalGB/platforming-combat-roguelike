using UnityEngine;
using System.Collections;

public class DOTBlast : CloseBlast
{
	
	public int intervals;

	override public void burstEffect(Transform blast, Transform target)
	{
		if(target.GetComponent<GameActor>()) {
			target.SendMessage("ApplyDOT",new Vector2(effectSize,intervals));
			target.SendMessage("SetColor",Color.red);
		}
	}

	override public void applyPassive(Transform player)
	{
		
	}
	
	override public void undoPassive(Transform player)
	{
		
	}


}

