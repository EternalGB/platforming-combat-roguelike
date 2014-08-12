using UnityEngine;
using System.Collections.Generic;

public class InfiniteSpawner : Spawner
{
	
	override protected void Spawn(Vector2 pos)
	{
		Create (pos);
	}
	
	
}

