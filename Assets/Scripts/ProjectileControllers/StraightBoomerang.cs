using UnityEngine;
using System.Collections;

public class StraightBoomerang : PoolableProjectile
{

	public float spinSpeed;
	public float velocityLoss;
	public float origVelocity;



	void Update()
	{
		transform.right = Quaternion.AngleAxis(spinSpeed*Time.deltaTime,Vector3.forward)*transform.right;
	}

	void FixedUpdate()
	{
		if(origVelocity > 0)
			rigidbody2D.velocity -= new Vector2(velocityLoss*Time.fixedDeltaTime,0);
		else
			rigidbody2D.velocity += new Vector2(velocityLoss*Time.fixedDeltaTime,0);
		rigidbody2D.velocity = Vector3.ClampMagnitude(rigidbody2D.velocity,Mathf.Abs(origVelocity));
	}



	void OnTriggerEnter2D(Collider2D col)
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
	
}

