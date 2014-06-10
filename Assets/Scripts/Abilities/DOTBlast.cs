using UnityEngine;
using System.Collections;

public class DOTBlast : CloseBlast
{
	
	public int intervals;

	override public void burstEffect(Transform blast, Transform target)
	{
		target.SendMessage("ApplyDOT",new Vector2(effectSize,intervals));
	}

}

