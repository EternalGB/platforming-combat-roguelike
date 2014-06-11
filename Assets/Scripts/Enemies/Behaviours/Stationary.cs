using UnityEngine;
using System.Collections;

public class Stationary : BaseEnemyBehaviour
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

