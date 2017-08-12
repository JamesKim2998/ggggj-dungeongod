using UnityEngine;

public class BaseEnemyController : EnemyController
{
    private void Start()
    {
        character = GetComponent<Enemy>();
        hero = FindObjectOfType<Hero>();
    }

    public override void NextTurn()
    {
        int xDistance, yDistance;

        if (Coord.distance(hero.coord, character.coord) <= detectDistance)
            character.condition.type = ConditionType.COMBAT;
        if (character.condition.type == ConditionType.WAIT)
            return;
        if (character.condition.type == ConditionType.COMBAT)
        {
            Dir[] moveDirection = new Dir[2];
            xDistance = character.coord.x - hero.coord.x;
            yDistance = character.coord.y - hero.coord.y;

            if (xDistance > 0)
                moveDirection[0] = Dir.Left;
            else if (xDistance < 0)
                moveDirection[0] = Dir.Right;
            if (yDistance > 0)
                moveDirection[1] = Dir.Down;
            else if (yDistance < 0)
                moveDirection[1] = Dir.Up;
            if (xDistance == 0)
                moveDirection[0] = moveDirection[1];
            if (yDistance == 0)
                moveDirection[1] = moveDirection[0];

            character.TryToMove(moveDirection[Random.Range(0, 2)]);
        }
    }
}