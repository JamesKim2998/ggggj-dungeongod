using System;
using UnityEngine;
using System.Collections;

public class Hero : Character
{
	public int level = 1;
	public int expNeeded = 10;

	public ConditionType? nextCondition;
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
			if (onHitExit != null)
				onHitExit();
		}

		else if (other.tag == "Equipment")
		{
            if (ItemManager.equipDic[other.GetComponent<EquipmentItem>().code].power >= ItemManager.equipDic[other.GetComponent<EquipmentItem>().code].power){
                ItemManager.heroEquipInfo.Remove(ItemManager.equipDic[other.GetComponent<EquipmentItem>().code].type);
                ItemManager.heroEquipInfo.Add(ItemManager.equipDic[other.GetComponent<EquipmentItem>().code].type, other.GetComponent<EquipmentItem>().code);
            }
			// TODO :  loot or ignore
		}

        else if (other.tag == "Consumable")
        {
            if ((int)other.GetComponent<ConsumableItem>().code <= 2) // 힐계열 아이템
                MainLogic.instance.hero.HP = Mathf.Min(MainLogic.instance.hero.HP + ItemManager.consumalbeDic[other.GetComponent<ConsumableItem>().code], MainLogic.instance.hero.maxHP);
            else //버프계열 아이템
                MainLogic.instance.hero.buffedTurn += ItemManager.consumalbeDic[other.GetComponent<ConsumableItem>().code];
            Destroy(other.gameObject);
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
			StartCoroutine(AudioManager.playSFX(Camera.main.gameObject.AddComponent<AudioSource>(), MainLogic.instance.audioManager.SFXs[4]));
	}

	public void Attack(Enemy enemy)
	{
		animation.Attack(enemy, callback: AttackCallback);
	}

	void AttackCallback(Character target)
	{
		var enemy = target as Enemy;
		enemy.getDamage(power, DiceRoll());

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
