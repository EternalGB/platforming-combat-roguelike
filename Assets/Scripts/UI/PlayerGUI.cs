using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AbilitiesController))]
[RequireComponent(typeof(PlayerController))]
public class PlayerGUI : MonoBehaviour
{

	public enum GUIMode
	{
		GAME,ABILITY_PREVIEW,ABILITY_MENU
	}

	public GUISkin skin;

	public Texture pauseOverlay;
	public Texture abilitiesOverlay;
	public float origWidth;
	public float origHeight;
	public Rect abBarArea;
	public GUIMode mode;
	Vector3 scale;
	float abIconSize = 100;
	float abSpacing = 20;

	public static bool paused = false;

	public Texture healthBarIcon;
	public Texture healthBarTexture;
	public Texture healthBarBacking;
	public Rect healthBarDefault;

	float upgradeButton = 0;

	public Texture abilityPreviewOverlay;
	Ability lastReceivedAbility;

	public Texture abMenuSelection;
	int abMenuSelected = 0;
	int abMenuRowSize = 5;
	public bool abMenuCanSelect = true;
	float selectionInterval = 0.15f;
	int numAbilities;
	float abMenuHeight = 0;
	Vector2 abMenuPosition = Vector2.zero;

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
		if(Input.GetButtonDown("Pause"))
			TogglePause();

		if(Input.GetButtonDown("AbilityMenu")) {
			if(mode != GUIMode.ABILITY_MENU) {
				TogglePause();
				mode = GUIMode.ABILITY_MENU;
			} else {
				TogglePause();
			}
		}

		if(paused)
			Time.timeScale = 0;
		else
			Time.timeScale = 1;

		upgradeButton = Input.GetAxisRaw("Upgrade");
		//TODO ability activates immediately after assigning
		if(mode == GUIMode.ABILITY_PREVIEW) {
			if(Input.GetButtonDown("A1")) {
				abCont.AddAbility(lastReceivedAbility,0);
				TogglePause();
			} else if(Input.GetButtonDown("A2")) {
				abCont.AddAbility(lastReceivedAbility,1);
				TogglePause();
			} else if(Input.GetButtonDown("A3")) {
				abCont.AddAbility(lastReceivedAbility,2);
				TogglePause();
				Input.ResetInputAxes();
			} else if(Input.GetButtonDown("A4")) {
				abCont.AddAbility(lastReceivedAbility,3);
				TogglePause();
			} else if(Input.GetButtonDown("Pause")) {
				abCont.AddAbility(lastReceivedAbility,-1);
				TogglePause();
			} else if(Input.GetButtonDown("Upgrade")) {
				if(Input.GetButtonDown("A1")) {
					abCont.UpgradeAbility(lastReceivedAbility,0);
					TogglePause();
				} else if(Input.GetButtonDown("A2")) {
					abCont.UpgradeAbility(lastReceivedAbility,1);
					TogglePause();
				} else if(Input.GetButtonDown("A3")) {
					abCont.UpgradeAbility(lastReceivedAbility,2);
					TogglePause();
				} else if(Input.GetButtonDown("A4")) {
					abCont.UpgradeAbility(lastReceivedAbility,3);
					TogglePause();
				}
			}
		} else if(mode == GUIMode.ABILITY_MENU) {
			if(abMenuCanSelect) {
				int hori = (int)Input.GetAxisRaw("Horizontal");
				int vert = -(int)Input.GetAxisRaw("Vertical");
				if(hori != 0 || vert != 0) {
					int newSelection = abMenuSelected + hori + vert*abMenuRowSize;
					if(newSelection >= 0 && newSelection < abCont.allAbilities.Count) {
						abMenuSelected = newSelection;
						abMenuCanSelect = false;
						StartCoroutine(Timers.CountdownRealtime(selectionInterval,EnableSelect));
					}
				}
			}

			Ability selectedAb = abCont.allAbilities[abMenuSelected];
			if(!abCont.IsActive(selectedAb)) {
				if(Input.GetButtonDown("A1")) {
					abCont.SetAbility(abMenuSelected,0);
				} else if(Input.GetButtonDown ("A2")) {
					abCont.SetAbility(abMenuSelected,1);
				} else if(Input.GetButtonDown("A3")) {
					abCont.SetAbility(abMenuSelected,2);
				} else if(Input.GetButtonDown ("A4")) {
					abCont.SetAbility(abMenuSelected,3);
				}
			}
		}

		if(Input.GetKeyDown("f2")) {
			GetAbility((Ability)GameObject.Find ("Boulder").GetComponent<Boulder>());
		}
	}



	public void EnableSelect()
	{
		abMenuCanSelect = true;
	}

	void TogglePause()
	{
		paused = !paused;
		abCont.ToggleInput();
		if(mode == GUIMode.ABILITY_PREVIEW || mode == GUIMode.ABILITY_MENU)
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
			for(int i = 0; i < abCont.activeAbilities.Length; i++) {
				Ability ab = abCont.activeAbilities[i];
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
			GUI.color = tmpColor;
			//draw the player's health bar
			float percentHealth = pCont.currentHealth/pCont.maxHealth;
			GUI.DrawTexture(new Rect(healthBarDefault.x,healthBarDefault.y
			                         ,healthBarDefault.height,healthBarDefault.height),
			                healthBarIcon);
			GUI.DrawTexture(new Rect(healthBarDefault.x + healthBarDefault.height,healthBarDefault.y,
			                         healthBarDefault.width,healthBarDefault.height),
			                healthBarBacking,ScaleMode.StretchToFill);
			GUI.DrawTexture(new Rect(healthBarDefault.x + healthBarDefault.height,healthBarDefault.y,
			                         healthBarDefault.width*percentHealth,healthBarDefault.height),
			                healthBarTexture,ScaleMode.StretchToFill);

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
			             "Press Q,W,E,R to assign\n\n" +
			             "Press LCTRL+(Q,W,E,R) to upgrade an ability\n\n" +
			             "Press (BACKSPACE) to manage abilities\n\n" +
			             "Press (ESC) to continue", skin.FindStyle("DescriptiveText"));
			GUI.EndGroup ();

			GUI.EndGroup ();
		} else if(mode == GUIMode.ABILITY_MENU) {
			//draw the abilities overlay and ability icons
			GUI.DrawTexture(new Rect(0,0,origWidth,origHeight),abilitiesOverlay);
			

			for(int i = 0; i < abCont.activeAbilities.Length; i++) {
				Ability ab = abCont.activeAbilities[i];
				if(ab != null && ab.icon != null) {
					GUI.DrawTexture(new Rect(abBarArea.x + (i+1)*abSpacing + i*abIconSize,
					                         abBarArea.y + abSpacing,
					                         abIconSize, abIconSize), ab.icon.texture);
				}
			}
			//draw the abilities area
			float iconSize = 100;
			float iconMargin = 30;
			numAbilities = abCont.allAbilities.Count;
			abMenuHeight = Mathf.Ceil(numAbilities/abMenuRowSize)*iconSize 
				+ (Mathf.Ceil(numAbilities/abMenuRowSize)+1)*iconMargin;
			Color tmpColor = GUI.color;
			GUI.BeginGroup(new Rect(50,50,720,480));
			GUI.DrawTexture(new Rect(0,0,720,480),abilityPreviewOverlay,ScaleMode.StretchToFill);
			abMenuPosition = GUI.BeginScrollView(new Rect(0,0,720,480),abMenuPosition,new Rect(0,0,720,abMenuHeight));
			for(int i = 0; i < abCont.allAbilities.Count; i++) {
				Ability ab = abCont.allAbilities[i];
				if(abCont.IsActive(ab)) {
					GUI.color = Color.gray;
				} else
					GUI.color = tmpColor;
				int x = i%abMenuRowSize;
				int y = i/abMenuRowSize;
				GUI.DrawTexture(new Rect((x+1)*iconMargin + x*iconSize,(y+1)*iconMargin + y*iconSize,iconSize,iconSize),
				                ab.icon.texture);
			}
			GUI.color = tmpColor;
			//draw the selection box
			int left = abMenuSelected%abMenuRowSize;
			int top = abMenuSelected/abMenuRowSize;
			GUI.DrawTexture(new Rect((left+1)*iconMargin + left*iconSize,(top+1)*iconMargin + top*iconSize,iconSize,iconSize),
			                abMenuSelection);
			GUI.EndScrollView();
			GUI.EndGroup();
			//draw the ability description area
			GUI.BeginGroup(new Rect(820,50,410,480));
			GUI.DrawTexture(new Rect(0,0,410,480),abilityPreviewOverlay,ScaleMode.StretchToFill);
			GUI.Label (new Rect(0,0,410,60),abCont.allAbilities[abMenuSelected].abilityName,skin.GetStyle("AbilityName"));
			GUI.Label(new Rect(0,60,410,30),"Active Effect",skin.GetStyle("SubTitle"));
			GUI.TextArea(new Rect(0,90,410,110), abCont.allAbilities[abMenuSelected].activeDescription.text, skin.GetStyle("DescriptiveText"));
			GUI.Label(new Rect(0,200,410,30),"Upgrade Effect",skin.GetStyle("SubTitle"));
			GUI.TextArea(new Rect(0,230,410,110), abCont.allAbilities[abMenuSelected].upgradeDescription.text, skin.GetStyle("DescriptiveText"));
			GUI.Label(new Rect(0,340,410,30),"Passive Effect",skin.GetStyle("SubTitle"));
			GUI.TextArea(new Rect(0,370,410,110), abCont.allAbilities[abMenuSelected].passiveDescription.text, skin.GetStyle("DescriptiveText"));
			GUI.EndGroup ();

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
		TogglePause();
		mode = GUIMode.ABILITY_PREVIEW;
		lastReceivedAbility = ab;
	}

}

