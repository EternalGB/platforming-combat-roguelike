using UnityEngine;
using System.Collections;

public class AbilityPickup : MonoBehaviour
{

	public Ability ability;
	public SpriteRenderer spriteRenderer;

	void Start()
	{
		if(ability != null)
			spriteRenderer.sprite = ability.icon;
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		//double check that the player is picking this up
		if(col.gameObject.name == "Player") {
			col.gameObject.SendMessage("GetAbility",ability);
			Destroy(gameObject);
		}
	}

	void SetAbility(Ability ab)
	{
		ability = ab;
		if(ability != null)
			spriteRenderer.sprite = ability.icon;
	}

	void SetSprite(Texture2D texture)
	{
		spriteRenderer.sprite = Sprite.Create(texture, new Rect(0,0,100,100),new Vector2(50,50),100);
	}

}

