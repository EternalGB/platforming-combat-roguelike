using UnityEngine;
using System.Collections;
using System;

public class OrbController : MonoBehaviour
{

	public Transform owner;
	public float radius;
	public float rotationSpeed;
	public Vector3 offset = Vector2.right;
	public LayerMask destructionMask;
	public Action<Transform, Transform> onCollision;
	protected LayerMask onCollisionTargets;
	public bool IsOn
	{
		get; private set;
	}



	void Start()
	{
		orbMove();
		//TurnOff();
	}

	void Update()
	{
		orbMove();
	}

	void orbMove()
	{
		if(owner) {
			offset = Quaternion.Euler(0,0,-rotationSpeed*Time.deltaTime)*offset;
			transform.position = owner.position + offset.normalized*radius;
		}
	}

	public void TurnOn()
	{
		collider2D.enabled = true;
		renderer.enabled = true;
		IsOn = true;
	}

	public void TurnOff()
	{
		collider2D.enabled = false;
		renderer.enabled = false;
		IsOn = false;
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
			TurnOff();
		}
	}

	public void SetOnCollision(Action<Transform, Transform> action, LayerMask targets)
	{
		onCollision = action;
		onCollisionTargets = targets;
	}
	
	void ResetOnCollision()
	{
		onCollision = null;
	}

}

