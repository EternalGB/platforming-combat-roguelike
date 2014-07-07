using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DiscreteBlast : BlastController 
{

	void OnTriggerEnter2D(Collider2D col)
	{

		if(onHitByBlast != null && (blastTargets.value &1 << col.gameObject.layer) != 0) {
			onHitByBlast(transform, col.transform);
		}

	}
	

}
