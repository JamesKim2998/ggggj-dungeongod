﻿using System;
using UnityEngine;
using System.Collections;

public class Enemy : Character
{
    public int exp = 5;
    public bool isLootable = false;

    public EnemyReaction reaction;
    public bool raged = false;

    public void Looted()
    {
        isLootable = false;
    }

    public virtual void Attack(Hero hero)
    {
        if (raged)
        {
            hero.getDamage((int)(power * 1.2f));
            raged = false;
        }
        else
            hero.getDamage(power);

    }

    // OVERRIDE FUNCTIONS
    public override void OnCantMove(GameObject target)
    {
        Hero hero = target.GetComponent<Hero>();

        if (hero!= null)
        {
            Attack(hero);
        }
    }

    public override void Die()
    {
        isLootable = true;
    }
}
