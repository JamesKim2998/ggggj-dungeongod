using UnityEngine;

public class EnemyController : MonoBehaviour
{
    PathFinder pathFinder;
    protected Enemy character;
    protected Hero hero;

    public int detectDistance = 3;
    public int chaseDistance = 4;
    public int attackRange = 1;
    public int countdown = 0;

	void Awake()
	{
		pathFinder = MainLogic.instance.dungeon.currentFloor.pathFinder;
	}

    private void OnEnable()
    {
        character = GetComponent<Enemy>();
    }

    public virtual void setCondition(ConditionType type, int countDown)
    {
        character.condition = type;
        this.countdown = countDown;
    }

    public virtual void UpdateCondition(Hero hero) { 
    
        if (character.prePanic == true) {
            character.condition = ConditionType.PANIC;
            countdown = 3;
            character.prePanic = false;
        }

        if (character.condition == ConditionType.PANIC)
        {
            countdown--;
            if (countdown <= 0)
            {
                character.condition = character.defaultCondition;
                countdown = 0;
            }
            return;
        }

        if (Coord.distance(hero.coord, character.coord) <= detectDistance)
        {
            character.condition = ConditionType.COMBAT;
        }
        else if (Coord.distance(hero.coord, character.coord) >= chaseDistance)
        {
            character.condition = character.defaultCondition;
        }
    }

    //Actions
    public virtual void Explore()
    {
        int number = Random.Range(1, 5);
        Dir dir_Itr = Dir.Up;
        for (int i = 0; i < number; i++)
        {
            dir_Itr = dir_Itr.Clockwise();
            character.TryToMove(dir_Itr);
        }
    }

    public virtual void Wait()
    {
        character.TryToMove(Dir.Stay);
    }

    public virtual void RunAway(Coord Source)
    {
        character.TryToMove(
            pathFinder.FindPath(character.coord, Source)
            .Reverse());
    }

    public virtual void Follow(Coord Source)
    {
        character.TryToMove(
            pathFinder.FindPath(character.coord, Source));

    }

    public virtual void Combat(Hero hero)
    {
        if (Coord.distance(hero.coord, character.coord) == 1)
        {
            //melee attack
            Follow(hero.coord);
        }
        else if (Coord.distance(hero.coord, character.coord) <= attackRange)
        {
            //ranged attack
            character.Attack(hero);
        }
        else
        {
            Follow(hero.coord);
        }
    }

    //Next Turn
    public virtual void NextTurn()
    {
        var hero = MainLogic.instance.hero;
        UpdateCondition(hero);

        switch (character.condition)
        {
            case ConditionType.EXPLORE: Explore(); break;
            case ConditionType.WAIT: Wait(); break;
            case ConditionType.COMBAT: Combat(hero); break;
            case ConditionType.RUNAWAY:
            case ConditionType.PANIC: RunAway(hero.coord); break;
            case ConditionType.GATHER: Follow(character.initialCoord); break;
            default: break;
        }
    }
}