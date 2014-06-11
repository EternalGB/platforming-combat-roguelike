using UnityEngine;
using System.Collections;

public class FixedPointText : MonoBehaviour 
{

	public string text;
	public GUIStyle style;
	public float width;
	float height;
	Vector2 screenPos;
	GUIContent content;

	void Start()
	{
		content = new GUIContent(text);
		height = style.CalcHeight(content,width);
	}

	void OnGUI()
	{
		screenPos = Camera.main.WorldToScreenPoint(transform.position);
		GUI.Label(new Rect(screenPos.x,Screen.height - (screenPos.y+height),width,height),content,style);
	}

}
