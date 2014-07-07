using UnityEngine;
using System.Collections;

public abstract class PoolableObject : MonoBehaviour
{

	protected void OnEnable()
	{
		Init();
	}

	protected abstract void Init();

	public void Destroy()
	{
		gameObject.SetActive (false);
	}

	protected abstract void PreDestroy();

	protected void OnDisable()
	{
		Reset();
		CancelInvoke();
	}

	protected abstract void Reset();


}

