using UnityEngine;
using System.Collections;

public abstract class BaseEnemyBehaviour : GameActor
{

	protected void Start()
	{
		base.Start ();
		ActiveEnemiesSingleton.Instance.AddEnemy(gameObject);
	}

	void OnDestroy()
	{
		ActiveEnemiesSingleton.Instance.RemoveEnemy(gameObject);
	}

}

