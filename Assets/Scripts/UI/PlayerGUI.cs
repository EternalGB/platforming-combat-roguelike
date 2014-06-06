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

	public Texture abilityPreviewOverlay;
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
		upgradeButton = Input.GetAxisRaw("Upgrade");
		if(Input.GetButtonDown("Pause"))
			TogglePause();


		if(paused)
			Time.timeScale = 0;
		else
			Time.timeScale = 1;


		if(Input.GetKeyDown("f2")) {
			GetAbility((Ability)GameObject.Find ("Boulder").GetComponent<Boulder>());
		}
	}

	void TogglePause()
	{
		paused = !paused;
		if(mode == GUIMode.ABILITY_PREVIEW)
			mode = GUIMode.GAME;
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
					                         abIconSize, abIconSize), ab.icon.texture);
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
			float groupWidth = 880;
			float groupHeight = 720;
			float titleHeight = 100;
			//main group
			GUI.BeginGroup(new Rect(200,0,880,720));
			GUI.DrawTexture(new Rect(0,0,880,720),abilityPreviewOverlay);
			//title
			GUI.Label (new Rect(0,0,400,titleHeight),
			           lastReceivedAbility.abilityName,skin.FindStyle("AbilityName"));
			//icon
			DrawRotatedTexture(new Rect(200,titleHeight+50,200,200),lastReceivedAbility.icon.texture,-10);
			//Description group
			GUI.BeginGroup (new Rect(500,titleHeight,380,groupHeight - titleHeight));
			GUI.Label (new Rect(0,0,380,50),"Active", skin.FindStyle("SubTitle"));
			GUI.TextArea(new Rect(0,50,380,150),
			             lastReceivedAbility.activeDescription.text, skin.FindStyle("DescriptiveText"));
			GUI.Label (new Rect(0,200,380,50),"Upgrade", skin.FindStyle("SubTitle"));
			GUI.TextArea(new Rect(0,250,380,150),
			             lastReceivedAbility.upgradeDescription.text, skin.FindStyle("DescriptiveText"));
			GUI.Label (new Rect(0,400,380,50),"Passive", skin.FindStyle("SubTitle"));
			GUI.TextArea(new Rect(0,450,380,150),
			             lastReceivedAbility.passiveDescription.text, skin.FindStyle("DescriptiveText"));
			GUI.EndGroup ();
			//Help text group
			GUI.BeginGroup (new Rect(100,500,500,220));
			GUI.TextArea(new Rect(0,0,500,220),
			             "Press (ABILITY) to assign\n\n" +
			             "Press (UPG)+(AB) to upgrade an ability\n\n" +
			             "Press (MENU) to manage abilities\n\n" +
			             "Press (PAUSE) to continue", skin.FindStyle("DescriptiveText"));
			GUI.EndGroup ();

			GUI.EndGroup ();

			//TODO
			//Draw ability icon
			//Draw descriptive text
			//Draw help text
		}
		if(paused)
			GUI.DrawTexture(new Rect(0,0,origWidth,origHeight), pauseOverlay);

		GUI.matrix = lastMat;
	}
		
	void DrawRotatedTexture(Rect loc, Texture texture, float angle)
	{
		Matrix4x4 mat = GUI.matrix;
		Matrix4x4 rot = Matrix4x4.TRS(new Vector3(-loc.width/2,loc.height/2),Quaternion.Euler(0,0,angle),Vector3.one);
		GUI.matrix = rot*mat;
		GUI.DrawTexture(loc,texture);
		GUI.matrix = mat;
	}

	void GetAbility(Ability ab)
	{
		mode = GUIMode.ABILITY_PREVIEW;
		lastReceivedAbility = ab;
		paused = true;

	}

}

