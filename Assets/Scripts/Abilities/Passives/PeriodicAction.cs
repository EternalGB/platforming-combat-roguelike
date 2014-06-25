using UnityEngine;
using System.Collections;

public class PeriodicAction : MonoBehaviour
{

	System.Action<Transform, Transform> action;
	float period;
	float activateChance;

	public void init(System.Action<Transform, Transform> action, float period, float activateChance)
	{
		this.action = action;
		this.period = period;
		this.activateChance = activateChance;
		StartCoroutine(Timers.Countdown<Transform, Transform>(period,activate,transform,null));
	}
	
	void activate(Transform t1, Transform t2)
	{
		if(Random.value < activateChance) {
			action(t1,t2);
		}
		StartCoroutine(Timers.Countdown<Transform, Transform>(period,activate,transform,null));
	}


	//TODO stop coroutine?

}

