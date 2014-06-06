using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilityBox : MonoBehaviour 
{

	public GameObject pickupPrefab;
	public Ability ability;
	public Sprite disabledSprite;
	public Sprite enabledSprite;
	public float enableRadius;
	public bool openable;
	public LayerMask canOpen;
	public float ejectionForce;
	SpriteRenderer spriteRenderer;

	void Start()
	{
		openable = false;
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		if(ability != null && Physics2D.OverlapCircle(transform.position,enableRadius,canOpen)) {
			Enable();
		} else {
			Disable();
		}

		if(ability != null && openable && Input.GetButtonDown("OpenBox")) {
			GameObject pickup = (GameObject)Instantiate(pickupPrefab,transform.position,Quaternion.identity);
			pickup.SendMessage("SetAbility",ability);
			pickup.rigidbody2D.AddForce(RandomOnCircleAngles(-45,45)*ejectionForce);
			Destroy(gameObject);
		}
	}

	Vector2 RandomOnCircleAngles(float minAngle, float maxAngle)
	{
		float angle = Random.Range(minAngle,maxAngle);
		Vector2 vec = Vector2.up;
		vec = Quaternion.AngleAxis(angle,Vector3.forward)*vec;
		//Debug.DrawLine(transform.position,(Vector2)transform.position+vec*ejectionForce,Color.red,10);
		return vec;
	}

	void Enable()
	{
		spriteRenderer.sprite = enabledSprite;
		openable = true;
	}

	void Disable()
	{
		spriteRenderer.sprite = disabledSprite;
		openable = false;
	}
		 

}
