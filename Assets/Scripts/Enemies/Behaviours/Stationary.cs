using UnityEngine;
using System.Collections;

public class Stationary : BaseEnemyBehaviour
{

	override protected Vector2 movingDir()
	{
		return Vector2.zero;
	}
	
	override protected bool isStrafing()
	{
		return false;
	}	

}

