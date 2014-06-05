using UnityEngine;
using System.Collections;

public class DamageOnCollision : MonoBehaviour
{

	public LayerMask targets;
	public float damage;
	public float force;

	void OnCollisionEnter2D(Collision2D col)
	{
		if((targets.value &1 << col.gameObject.layer) != 0) {
			col.transform.SendMessage("Damage",damage);

			if(col.rigidbody != null) {
				Vector2 pos2D = new Vector2(transform.position.x,transform.position.y);
				Vector2 forceDir = (col.contacts[0].point - pos2D).normalized;
				col.rigidbody.AddForce(forceDir*force);
			}
		}
	}

}

