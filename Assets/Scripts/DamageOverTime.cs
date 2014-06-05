using UnityEngine;
using System.Collections;
using System;

public class DamageOverTime 
{

	const float DOTTickInterval = 0.5f;
	bool stopped = false;
	int intervals;
	float damage;
	Action<float> damageAction;
		
	public DamageOverTime(int intervals, float damage, Action<float> callback)
	{
		this.intervals = intervals;
		this.damage = damage;
		this.damageAction = callback;
	}

	public IEnumerator Start()
	{
		for(float i = 0; i < intervals; i++) {
			if(stopped)
				break;
			yield return new WaitForSeconds(DOTTickInterval);
			damageAction(damage);
		}
		stopped = true;
	}

	public void Stop()
	{
		stopped = true;
	}

	public bool isStopped()
	{
		return stopped;
	}





}

