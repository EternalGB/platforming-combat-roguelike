using UnityEngine;
using System.Collections;

public class ExitController : MonoBehaviour
{

	public Sprite lockedSprite;
	public Sprite unlockedSprite;
	SpriteRenderer spriteRenderer;
	bool locked = true;

	// Use this for initialization
	void Start ()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(!locked && col.gameObject.name == "Player") {
			//change level
		}
	}

	void Lock()
	{
		locked = true;
		spriteRenderer.sprite = lockedSprite;
	}

	void Unlock()
	{
		locked = false;
		spriteRenderer.sprite = unlockedSprite;
	}
	
}

