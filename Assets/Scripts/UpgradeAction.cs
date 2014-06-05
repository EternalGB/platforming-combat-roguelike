using UnityEngine;
using System.Collections;
using System;

public class UpgradeAction
{

	public LayerMask targets;
	public Action<Transform> locationAction;
	public Action<Transform, Transform> targetAction;

	public UpgradeAction(Action<Transform> function) {
		this.locationAction = function;
	}

	public UpgradeAction(Action<Transform, Transform> action, LayerMask target) {
		this.targetAction = action;
		this.targets = target;
	}

}

