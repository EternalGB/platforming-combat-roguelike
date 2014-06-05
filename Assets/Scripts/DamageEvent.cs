using UnityEngine;
using System.Collections;

public class DamageEvent
{

	public float amount;
	public Vector2 center;

	public DamageEvent(float amount, Vector2 center)
	{
		this.amount = amount;
		this.center = center;
	}

}

