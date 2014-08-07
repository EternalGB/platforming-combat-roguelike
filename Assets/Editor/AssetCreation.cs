using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System;

public class AssetCreation 
{

	[MenuItem("Assets/Create/Improvement")]
	public static void createImprovement()
	{
		CreateAsset<Improvement>();
	}

	public static void CreateAsset<T> () where T : ScriptableObject
	{
		T asset = ScriptableObject.CreateInstance<T> ();
		
		string path = AssetDatabase.GetAssetPath (Selection.activeObject);
		if (path == "") 
		{
			path = "Assets";
		} 
		else if (Path.GetExtension (path) != "") 
		{
			path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
		}
		
		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/New " + typeof(T).ToString() + ".asset");
		
		AssetDatabase.CreateAsset (asset, assetPathAndName);
		
		AssetDatabase.SaveAssets ();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = asset;
	}

}
