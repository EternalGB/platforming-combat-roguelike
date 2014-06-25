using UnityEngine;
using System.Collections;

public class PeriodicAction : MonoBehaviour
{

	System.Action<Transform> action;
	float period;
	float activateChance;

	public void init(System.Action<Transform> action, float period, float activateChance)
	{
		this.action = action;
		this.period = period;
		this.activateChance = activateChance;
		StartCoroutine(Timers.Countdown<Transform>(period,activate,transform));
	}
	
	void activate(Transform t1)
	{
		if(Random.value < activateChance) {
			action(t1);
		}
		StartCoroutine(Timers.Countdown<Transform>(period,activate,transform));
	}


	//TODO stop coroutine?

}

