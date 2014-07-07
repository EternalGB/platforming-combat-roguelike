using UnityEngine;
using System.Collections.Generic;
using System;

public class PoolableProjectile : MonoBehaviour
{

	public float bulletLifetime = 3f;
	public LayerMask destructionMask;
	public Action<Transform> onDestroy;
	public Action<Transform, Transform> onCollision;
	protected LayerMask onCollisionTargets;
	List<Collider2D> ignored;

	protected virtual void OnEnable()
	{
		if(bulletLifetime > 0)
			Invoke("Destroy",bulletLifetime);
	}

	public void Destroy()
	{
		if(onDestroy != null) {
			onDestroy(transform);
		}
		gameObject.SetActive (false);
	}

	protected virtual void OnDisable()
	{
		ResetOnDestroy();
		ResetOnCollision();
		ResetIgnores();
		CancelInvoke();
	}

	protected virtual void OnCollisionEnter2D(Collision2D col)
	{
		CollisionHandler(col.collider);
	}

	protected virtual void OnTriggerEnter2D(Collider2D col)
	{
		CollisionHandler(col);
	}

	protected void CollisionHandler(Collider2D col)
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

	public void IgnoreCollider(Collider2D col)
	{

		Physics2D.IgnoreCollision(collider2D,col);
		if(ignored == null) {
			ignored = new List<Collider2D>();
		}
		ignored.Add(col);
		//ignore each child collider as well
		Collider2D[] childCols = col.GetComponentsInChildren<Collider2D>();
		foreach(Collider2D childCol in childCols) {
			Physics2D.IgnoreCollision(collider2D,childCol);
			ignored.Add (childCol);
		}
	}

	void ResetIgnores()
	{
		if(ignored != null) {
			foreach(Collider2D col in ignored)
				Physics2D.IgnoreCollision(collider2D,col,false);
			ignored.Clear();
		}
	}

	public void SetOnDestroy(System.Action<Transform> action, LayerMask targets)
	{
		onDestroy = action;
		destructionMask = targets;
	}

	void SetOnDestroy(UpgradeAction ua)
	{
		SetOnDestroy(ua.locationAction,ua.targets);
	}

	void ResetOnDestroy()
	{
		onDestroy = null;
	}

	public void SetOnCollision(System.Action<Transform, Transform> action, LayerMask targets)
	{
		onCollision = action;
		onCollisionTargets = targets;
	}

	void SetOnCollision(UpgradeAction ua)
	{
		SetOnCollision(ua.targetAction, ua.targets);
	}

	void ResetOnCollision()
	{
		onCollision = null;
	}

}

