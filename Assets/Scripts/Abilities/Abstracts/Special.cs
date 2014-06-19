using UnityEngine;
using System.Collections;

public abstract class Special : Ability
{


	void Start()
	{
		base.Start();
		if(upgrade != null) {
			upgradeAbility(upgrade);
		}
	}

}

