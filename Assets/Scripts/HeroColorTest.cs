using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroColorTest : MonoBehaviour
{
	public void Test()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			DestroyImmediate(transform.GetChild(i).gameObject);
		}
		var hero = HeroFactory.InstantiateRandom();
		hero.transform.SetParent(transform, false);
	}
}
