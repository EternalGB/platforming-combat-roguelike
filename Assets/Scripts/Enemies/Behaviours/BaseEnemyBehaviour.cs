using UnityEngine;
using System.Collections;

public abstract class BaseEnemyBehaviour : GameActor
{

	public GameObject xpPrefab;
	ObjectPool xpPool;
	public Vector2 xpDropRange;

	protected void Start()
	{
		base.Start ();
		xpPool = PoolManager.Instance.GetPoolByRepresentative(xpPrefab);
		ActiveEnemiesSingleton.Instance.AddEnemy(gameObject);
	}

	void OnDestroy()
	{
		DropXP();
		ActiveEnemiesSingleton.Instance.RemoveEnemy(gameObject);
	}

	void DropXP()
	{
		int amount = Random.Range((int)xpDropRange.x,(int)xpDropRange.y);
		for(int i = 0; i < amount; i++) {
			GameObject xp = xpPool.getPooled();
			XPDropController xpCont = xp.GetComponent<XPDropController>();
			xp.SetActive(true);
			xp.transform.position = transform.position;
			xpCont.SetValue(1);
		}
	}

}

