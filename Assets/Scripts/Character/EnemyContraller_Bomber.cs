using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContraller_Bomber : EnemyController {
    
    public int bombCountdown = 5;
    public bool bombOn = false;

    public override void UpdateCondition(Hero hero)
    {
        if(bombOn)
        {
            bombCountdown--;
        }
        base.UpdateCondition(hero);
    }

    public virtual void KABOOM(Hero hero)
    {
        if (Coord.distance(hero.coord, character.coord) <= 1)
            hero.getDamage(0, 12);
        character.Die();
    }

    public override void Combat(Hero hero)
    {
        bombOn = true;
        if (Coord.distance(hero.coord, character.coord) > 1)
            Follow(hero.coord);
    }

    public override void NextTurn()
    {
        var hero = MainLogic.instance.hero;
        if (bombCountdown <= 0)
            KABOOM(hero);
        base.NextTurn();
    }

}
