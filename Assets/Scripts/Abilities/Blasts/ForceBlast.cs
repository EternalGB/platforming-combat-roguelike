using UnityEngine;
using System.Collections;

public class ForceBlast : CloseBlast
{

	override public void burstEffect(Transform blast, Transform target)
	{
		if(target.rigidbody2D != null) {
			Vector3 forceDir = target.position - blast.position;
			Vector3 force = forceDir.normalized*effectSize;//*1/Mathf.Pow(forceDir.magnitude,2);
			target.rigidbody2D.AddForce(force);
		}
	}

}

