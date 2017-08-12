using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSelf : MonoBehaviour
{
	public float delay = 0.5f;

	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable()
	{
		Invoke("SelfDisable", delay);
	}

	void SelfDisable()
	{
		gameObject.SetActive(false);
	}
}
