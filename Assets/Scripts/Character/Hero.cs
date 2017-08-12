using System;
using UnityEngine;
using System.Collections;

public class Hero : Character
{
	public int level = 1;
	public int expNeeded = 10;

	public int visibleDistance = 8;

	public bool buffed = false;
	public int buffedTurn;

	public event System.Action onHitExit;
	public event System.Action onDead;

	private void OnTriggerEnter(Collider other)
	{
		var tag = other.GetComponent<ObjectTag>();
		if (tag != null && tag.type == ObjectType.DOWN_STAIR)
		{
			if (onHitExit != null)
				onHitExit();
		}

		else if (other.tag == "Equipment")
		{
			// TODO :  loot or ignore
		}
	}

	public void checkBuffEnded()
	{
		if (buffedTurn <= 0)
			buffed = false;
	}

	public void getEXP(int value)
	{
		expNeeded -= value;
		CheckLevelUp();
	}

	public void CheckLevelUp()
	{
		while (expNeeded <= 0)
		{
			//TODO LVL up
			level++;
			expNeeded += 10 * level;
			this.maxHP++;
			this.HP++;
		}
	}

	public void Attack(Enemy enemy)
	{
		enemy.getDamage(power);

		if (enemy.IsDead() && enemy.isLootable)
		{
			enemy.Looted();
			getEXP(enemy.exp);
		}
	}

	// OVERRIDE FUNCTIONS
	public override void OnCantMove(GameObject target)
	{
		Enemy enemy = target.GetComponent<Enemy>();

		if (enemy != null)
		{
			Attack(enemy);
		}
	}

	public override void Die()
	{
		var skeletonPrefab = Resources.Load<GameObject>("Hero/Dead Skeleton");
		var position = transform.position;
		var rotation = transform.rotation;
		var parent = MainLogic.instance.dungeon.currentFloor.transform;
		Instantiate(skeletonPrefab, position, rotation, parent);
		Destroy(gameObject);
		if (onDead != null) onDead();
	}
}