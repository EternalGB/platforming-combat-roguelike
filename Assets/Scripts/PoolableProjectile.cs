using UnityEngine;
using System.Collections.Generic;
using System;

public class PoolableProjectile : MonoBehaviour
{

	public float bulletLifetime = 3f;
	public LayerMask destructionMask;
	public Action<Transform> onDestroy;
	public Action<Transform, Transform> onCollision;
	LayerMask onCollisionTargets;
	List<Collider2D> ignored;

	void OnEnable()
	{
		Invoke("Destroy",bulletLifetime);
	}

	void Destroy()
	{
		gameObject.SetActive (false);
	}

	void OnDisable()
	{
		if(onDestroy != null)
			onDestroy(transform);
		ResetOnDestroy();
		ResetOnCollision();
		ResetIgnores();
		CancelInvoke();
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if(onCollision != null && (onCollisionTargets.value &1 << col.gameObject.layer) != 0) {
			onCollision(transform, col.transform);
		}

		//print(col.gameObject.layer + " " + destructionMask.value);
		if((destructionMask.value & 1 << col.gameObject.layer) != 0) {
			if(col.gameObject.layer == LayerMask.NameToLayer("CollProj")
			   || col.gameObject.layer == LayerMask.NameToLayer ("NonCollProj"))
				col.gameObject.SendMessage("Destroy");
			Destroy();
		}
	}

	void IgnoreCollider(Collider2D col)
	{
		Physics2D.IgnoreCollision(collider2D,col);
		if(ignored == null) {
			ignored = new List<Collider2D>();
		}
		ignored.Add(col);
	}

	void ResetIgnores()
	{
		if(ignored != null) {
			foreach(Collider2D col in ignored)
				Physics2D.IgnoreCollision(collider2D,col,false);
			ignored.Clear();
		}
	}

	void SetOnDestroy(UpgradeAction ua)
	{
		onDestroy = ua.locationAction;
	}

	void ResetOnDestroy()
	{
		onDestroy = null;
	}

	void SetOnCollision(UpgradeAction ua)
	{
		onCollision = ua.targetAction;
		onCollisionTargets = ua.targets;
	}

	void ResetOnCollision()
	{
		onCollision = null;
	}

}

