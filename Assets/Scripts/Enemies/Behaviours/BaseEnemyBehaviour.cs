using UnityEngine;
using System.Collections;

public abstract class BaseEnemyBehaviour : GameActor
{

	public Vector2 xpDropRange;

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

