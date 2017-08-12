using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HeroColorTest))]
public class HeroColorTestEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (GUILayout.Button("Test"))
		{
			(target as HeroColorTest).Test();
		}
	}
}
