using UnityEngine;
using System.Collections;

public class MainGUI : MonoBehaviour
{

	public Texture abilitiesOverlay;
	public float origWidth;
	public float origHeight;
	Vector3 scale;

	void OnGUI()
	{
		scale.x = Screen.width/origWidth;
		scale.y = Screen.height/origHeight;
		scale.z = 1;
		Matrix4x4 lastMat = GUI.matrix;

		GUI.matrix = Matrix4x4.TRS (Vector3.zero,Quaternion.identity,scale);

		GUI.DrawTexture(new Rect(0,0,origWidth,origHeight),abilitiesOverlay);
		//GUI here

		GUI.matrix = lastMat;
	}
		
}

