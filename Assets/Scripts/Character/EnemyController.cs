using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public DungeonFloor dungeonFloor;
    protected PathFinder pathFinder;
    protected Enemy character;
    protected Hero hero;
    public int detectDistance = 4;

    private void OnEnable()
    {
        character = GetComponent<Enemy>();
    }

    public virtual void NextTurn()
    {
        pathFinder = MainLogic.instance.pathFinder;
        var hero = MainLogic.instance.hero;

        if (Coord.distance(hero.coord, character.coord) <= detectDistance)
            character.condition.type = ConditionType.COMBAT;
        if (character.condition.type == ConditionType.WAIT)
            return;
        if (character.condition.type == ConditionType.COMBAT)
        {
            character.TryToMove(pathFinder.FindPath(character.coord, hero.coord));
        }
    }
}