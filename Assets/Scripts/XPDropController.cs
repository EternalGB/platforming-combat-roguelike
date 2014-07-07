using UnityEngine;
using System.Collections;

public class XPDropController : PoolableObject
{

	public Transform player;
	public float speed;
	public int value = 1;
	
	protected override void Init ()
	{
		player = PlayerController.GlobalPlayerInstance.transform;
		ScaleToValue();
	}

	void ScaleToValue()
	{
		transform.localScale = new Vector3(value,value,value);
	}

	protected override void PreDestroy ()
	{

	}

	protected override void Reset ()
	{
		value = 1;
	}

	public void SetValue(int value)
	{
		this.value = value;
		ScaleToValue();
	}

	// Update is called once per frame
	void Update ()
	{
		Vector3 dir = (player.position - transform.position).normalized;
		transform.position += dir*speed*Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.name == "Player") {
			col.gameObject.GetComponent<PlayerController>().GetXP(value);
			Destroy();
		}
	}
}

