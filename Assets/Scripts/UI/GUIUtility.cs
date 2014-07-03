using UnityEngine;
using System.Collections;

public class GUIUtility
{

	public static void PointStripDisplay(Rect position, float imageSize, int length,  
	                                     int selectedIndex, Texture on, Texture off)
	{
		GUI.BeginGroup(position);
		Rect pos = new Rect(0,0, imageSize, imageSize);
		Texture tex;
		for(int i = 0; i < length; i++) {
			pos.x = i*imageSize;
			if(i <= selectedIndex)
				tex = on;
			else
				tex = off;
			GUI.DrawTexture(pos,tex);
		}
		GUI.EndGroup();
	}

}

