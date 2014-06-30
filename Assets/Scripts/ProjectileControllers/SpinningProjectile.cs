using UnityEngine;
using System.Collections;

public class SpinningProjectile: PoolableProjectile
{

	public float spinSpeed;

	void Update()
	{
		transform.right = Quaternion.AngleAxis(spinSpeed*Time.deltaTime,Vector3.forward)*transform.right;
	}
	
}

