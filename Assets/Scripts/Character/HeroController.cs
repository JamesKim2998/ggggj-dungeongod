using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    public DungeonFloor dungeonFloor;
    protected PathFinder pathFinder;
    protected Hero character;
    protected List<Enemy> enemies;

    protected int chaseDistance = 8;

    public Coord targetCoord = new Coord();
    public int countdown = 0;

	void Awake()
	{
		character = GetComponent<Hero>();
	}

    public virtual void setCondition(ConditionType type, int countDown)
    {
        character.condition = type;
        this.countdown = countDown;
    }

    public virtual void UpdateCondition(List<Enemy> enemies)
    {
        pathFinder = MainLogic.instance.pathFinder;

        if (character.preParalyzed == true)
        {
            character.condition = ConditionType.PARALYZED;
            countdown = Random.Range(1,3);
            character.preParalyzed = false;
            character.prePanic = false;
        }

        if ( character.condition == ConditionType.PARALYZED )
        {
            countdown--;
            if (countdown <= 0 )
            {
                character.condition = character.defaultCondition;
                countdown = 0;
            }
            return;
        }

        if ( character.prePanic == true )
        {
            character.condition = ConditionType.PANIC;
            countdown = 4;
            character.prePanic = false;
        }

        if ( character.condition == ConditionType.PANIC )
        {
            countdown--;
            if ( countdown <= 0 )
            {
                character.condition = character.defaultCondition;
                countdown = 0;
            }
            return;
        }

        foreach( Enemy enemy in enemies)
        {
            if (pathFinder.isVisiableByHero(enemy.coord))
            {
                targetCoord = enemy.coord;
                if (character.HP > character.maxHP / 2)
                {
                    character.condition = ConditionType.COMBAT;
                }

                else
                {
                    character.condition = ConditionType.PANIC;
                }

            }
        }
    }

	Dir nextDirForDebug; // TODO: delete me
    public void NextTurn()
    {
		if (character == null)
		{
			return;
		}

        // TODO: Character 다음 행동.
		character.TryToMove(nextDirForDebug);
		nextDirForDebug = nextDirForDebug.Clockwise();
    }
}