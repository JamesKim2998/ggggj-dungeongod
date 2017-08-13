using System;
using UnityEngine;
using System.Collections;

public class Enemy : Character
{
	public int exp = 12;
	public bool isLootable = false;

	public bool prePanic = false;
	public int rage = 0;

	public Coord initialCoord;
	public ConditionType defaultCondition = ConditionType.GATHER;

	public EnemyReaction reaction;

	protected override void Awake()
	{
		base.Awake();
		condition = defaultCondition;
	}

	public void Looted()
	{
		isLootable = false;
	}

	public virtual void Attack(Hero hero)
	{
		if (animation != null)
		{
			animation.Attack(hero, callback: AttackCallback);
		}
		else
		{
			AttackCallback(hero);
		}
	}

	void AttackCallback(Character target)
	{
		var hero = target as Hero;
		hero.getDamage(GetPower(), DiceRoll() + rage);
		rage = 0;
	}

	// OVERRIDE FUNCTIONS
	public override void OnCantMove(GameObject target)
	{

		Hero hero = target.GetComponent<Hero>();

		if (hero != null)
		{
			Attack(hero);
		}
	}

	public override void getDamage(int power, int dice)
	{
		base.getDamage(power, dice);
		if (IsDead()) isLootable = true;
	}

	public override void Die()
	{
		Destroy(gameObject);
	}
}
