using System;
using UnityEngine;
using System.Collections;
using System.Linq;

public class Hero : Character
{
	public int level = 1;
	public int expNeeded = 10;

	public int fogDistance = 8;

	public ConditionType defaultCondition = ConditionType.EXPLORE;

	public bool buffed = false;
	public int buffedTurn;

	public event System.Action onHitExit;
	public event System.Action onDead;

	private void OnTriggerEnter(Collider other)
	{
		var tag = other.GetComponent<ObjectTag>();
		if (tag != null && tag.type == ObjectType.DOWN_STAIR)
		{
			animation.SetTrigger("Jumpdown");
			tag.GetComponent<Animator>().enabled = true;
		}

		else if (other.tag == "Equipment")
		{
			Debug.Log("Touched!");
			if (ItemManager.equipDic[other.GetComponent<EquipmentItem>().code].power >= ItemManager.equipDic[other.GetComponent<EquipmentItem>().code].power)
			{
				ItemManager.heroEquipInfo.Remove(ItemManager.equipDic[other.GetComponent<EquipmentItem>().code].type);
				ItemManager.heroEquipInfo.Add(ItemManager.equipDic[other.GetComponent<EquipmentItem>().code].type, other.GetComponent<EquipmentItem>().code);
				Destroy(other.gameObject);
				AudioManager.playSFX(this, MainLogic.instance.audioManager.SFXs[3]);
                EffectSpawner.SetEffect("PwUP", transform.position);
			}
			// TODO :  loot or ignore
		}

		else if (other.tag == "Consumable")
		{
			// if ((int)other.GetComponent<ConsumableItem>().code <= 2) // ���迭 ������
			MainLogic.instance.hero.HP = Mathf.Min(MainLogic.instance.hero.HP + ItemManager.consumalbeDic[other.GetComponent<ConsumableItem>().code], MainLogic.instance.hero.maxHP);
			// else //�����迭 ������
			// MainLogic.instance.hero.buffedTurn += ItemManager.consumalbeDic[other.GetComponent<ConsumableItem>().code];
			Destroy(other.gameObject);
			AudioManager.playSFX(this, MainLogic.instance.audioManager.SFXs[3]);
		}

	}

	public void JumpedDown()
	{
		onHitExit();
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

	public override int GetPower()
	{
		var equipPower = ItemManager.heroEquipInfo.Select(kvp => ItemManager.equipDic[kvp.Value].power).Sum();
		return power + equipPower;
	}

	public void CheckLevelUp()
	{
		bool levelup = expNeeded <= 0;
		while (expNeeded <= 0)
		{
			//TODO LVL up
			level++;
			expNeeded += 10 * level;
			this.maxHP++;
			this.HP++;
		}
		if (levelup)
		{
			var clip = MainLogic.instance.audioManager.SFXs[4];
			AudioManager.playSFX(this, clip, 0);
			EffectSpawner.SetEffect("LvUP", transform.position);
		}
	}

	public void Attack(Enemy enemy)
	{
		animation.Attack(enemy, callback: AttackCallback);
	}

	void AttackCallback(Character target)
	{
		var enemy = target as Enemy;
		enemy.getDamage(GetPower(), DiceRoll());

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

	public override bool Move(Dir dir, out RaycastHit hitInfo)
	{
		if (condition == ConditionType.PANIC || condition == ConditionType.RUNAWAY)
			moveType = "Panic";
		else
			moveType = "Move";
		return base.Move(dir, out hitInfo);
	}

	public override void Die()
	{
		if (onDead != null) onDead();
	}
}
