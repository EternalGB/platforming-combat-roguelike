using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AimedGunController : MonoBehaviour 
{
	
	public Transform pivot;
	public float fireSpeed;
	public float fireVelocity;
	float fireInterval;
	float fireTimer;
	bool canFire = true;
	ObjectPool bulletPool;

	public Vector2 facingDir
	{
		get
		{
			return transform.right;
		}
	}

	// Use this for initialization
	void Start () 
	{
		bulletPool = PoolManager.Instance.GetPoolByName("bulletPool");
	}



	// Update is called once per frame
	void Update () 
	{
		fireInterval = 1/fireSpeed;

		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 myPos = transform.position;
		float angle = signedAngle(transform.right,(mousePos - myPos).normalized);

		//print(angle);
		transform.Rotate(Vector3.forward,angle);
		/*
		if(Mathf.Sign (transform.parent.localScale.x) < 0) {
			Vector3 right = transform.right;
			right.x = -right.x;
			transform.right = right;
			Quaternion rot = transform.rotation;
			rot.w = -rot.w;
			transform.rotation = rot;
			transform.Rotate(Vector3.forward,180);
		}
		*/

		if(Input.GetMouseButton(0) && canFire) {
			canFire = false;
			FireBullet(transform.right);
			StartCoroutine(Timers.Countdown(fireInterval,EnableFire));
		}

		//print(transform.right);
		Debug.DrawLine(transform.position,myPos + facingDir.normalized*2,Color.red);
	}

	void FireBullet(Vector2 dir)
	{
		GameObject bullet = bulletPool.getPooled();
		bullet.transform.position = transform.position;
		bullet.transform.rotation = transform.rotation;
		bullet.SetActive(true);
		bullet.transform.Rotate(Vector3.forward,90);
		bullet.rigidbody2D.AddForce(dir*fireVelocity);
	}

	void EnableFire()
	{
		canFire = true;
	}

	private float signedAngle(Vector3 v1, Vector3 v2)
	{
		float angle = Vector2.Angle(v1,v2);
		Vector3 cross = Vector3.Cross (v1,v2);
		angle = Mathf.Sign(cross.z)*angle;
		return angle;

	}

	private float clockwiseAngle(Vector3 v1, Vector3 v2)
	{
		float angle = Vector3.Angle(v1,v2);
		Vector3 cross = Vector3.Cross (v1,v2);
		if(cross.z > 0)
			angle = 360-angle;
		return angle;
	}
	

}
