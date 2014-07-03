using UnityEngine;
using System.Collections;

public class Improvement : ScriptableObject
{


	public string fieldName;
	public string displayName;
	public float costPerPoint;
	public float pointValue;
	[System.NonSerialized]
	public int pointsAllocated;
	public int maxPoints;
	
}

