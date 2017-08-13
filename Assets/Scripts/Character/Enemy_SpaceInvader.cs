using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SpaceInvader : Enemy
{
	public override void getDamage(int power, int dice)
	{
		int powerDiff = power + dice - this.GetPower();
		if (powerDiff >= 12)
		{
			prePanic = true;
		}
		base.getDamage(power, dice);
	}
}
