using UnityEngine;
using System.Collections;

public class Stationary : GameActor
{

	override protected float horizontalMovingDir()
	{
		return 0;
	}
	
	override protected bool isStrafing()
	{
		return false;
	}	

}

