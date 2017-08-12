using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BitmapLoader))]
public class BitmapLoaderEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (GUILayout.Button("Load"))
		{
			(target as BitmapLoader).LoadBitmap();
		}
	}
}
