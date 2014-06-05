using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AbilitiesController))]
[RequireComponent(typeof(PlayerController))]
public class PlayerGUI : MonoBehaviour
{

	enum GUIMode
	{
		GAME,ABILITY_PREVIEW
	}

	public GUISkin skin;

	public Texture pauseOverlay;
	public Texture abilitiesOverlay;
	public float origWidth;
	public float origHeight;
	public Rect abBarArea;
	GUIMode mode;
	Vector3 scale;
	float abIconSize = 100;
	float abSpacing = 20;

	bool paused = false;

	public Texture healthBarIcon;
	public Texture healthBarTexture;
	public Rect healthBarDefault;

	float upgradeButton = 0;

	Ability lastReceivedAbility;

	AbilitiesController abCont;
	PlayerController pCont;

	void Start()
	{
		abCont = gameObject.GetComponent<AbilitiesController>();
		pCont = gameObject.GetComponent<PlayerController>();
		lastReceivedAbility = null;
		mode = GUIMode.GAME;
	}

	void Update()
	{
		if(paused)
			Time.timeScale = 0;
		else
			Time.timeScale = 1;

		upgradeButton = Input.GetAxisRaw("Upgrade");
	}

	void OnGUI()
	{


		scale.x = Screen.width/origWidth;
		scale.y = Screen.height/origHeight;
		scale.z = 1;
		Matrix4x4 lastMat = GUI.matrix;

		GUI.matrix = Matrix4x4.TRS (Vector3.zero,Quaternion.identity,scale);

		if(mode == GUIMode.GAME) {
			//draw the abilities overlay and ability icons
			GUI.DrawTexture(new Rect(0,0,origWidth,origHeight),abilitiesOverlay);

			Color tmpColor = GUI.color;
			for(int i = 0; i < abCont.abilities.Length; i++) {
				Ability ab = abCont.abilities[i];
				if(ab != null && ab.icon != null) {
					//greyout the ability if it's on cooldown
					if(!ab.canActivate)
						GUI.color = Color.gray;
					else
						GUI.color = tmpColor;
					GUI.DrawTexture(new Rect(abBarArea.x + (i+1)*abSpacing + i*abIconSize,
					                         abBarArea.y + abSpacing,
					                         abIconSize, abIconSize), ab.icon);
				}
			}

			//draw the player's health bar
			float percentHealth = pCont.currentHealth/pCont.maxHealth;
			GUI.DrawTexture(new Rect(healthBarDefault.x,healthBarDefault.y
			                         ,healthBarDefault.height,healthBarDefault.height),
			                healthBarIcon);
			GUI.DrawTexture(new Rect(healthBarDefault.x + healthBarDefault.height,healthBarDefault.y,
			                         healthBarDefault.width*percentHealth,healthBarDefault.height),
			                healthBarTexture);
			GUI.color = tmpColor;
		} else if(mode == GUIMode.ABILITY_PREVIEW) {
			if(paused)
				GUI.DrawTexture(new Rect(0,0,origWidth,origHeight), pauseOverlay);
			GUI.Label (new Rect(origWidth/2,20,200,50),lastReceivedAbility.abilityName,skin.FindStyle("AbilityName"));
			//TODO
			//Draw ability icon
			//Draw descriptive text
			//Draw help text
		}


		GUI.matrix = lastMat;
	}
		


	void GetAbility(Ability ab)
	{
		mode = GUIMode.ABILITY_PREVIEW;
		lastReceivedAbility = ab;
		paused = true;

	}

}

