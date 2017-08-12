using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GodPowerBar : MonoBehaviour
{
	public Image bar;
	public float powerMax;
	float curPower;
	float targetPower;

	void Awake()
	{
		bar.fillMethod = Image.FillMethod.Horizontal;
	}

	void Update()
	{
		if (powerMax == 0)
		{
			bar.fillAmount = 0;
			return;
		}

		if (curPower != targetPower)
			curPower = Mathf.Lerp(curPower, targetPower, Time.deltaTime);
		var newFillAmount = curPower / powerMax;
		if (bar.fillAmount != newFillAmount)
			bar.fillAmount = newFillAmount;
	}

	public void SetSmooth(float power)
	{
		targetPower = power;
	}

	public void SetImmediate(float power)
	{
		curPower = power;
		targetPower = power;
	}
}
