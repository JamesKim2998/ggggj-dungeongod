using UnityEngine;

public class EnemyGodTouchAction : GodTouchAction
{
    public Enemy enemy;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }
    public override void Act()
    {
        switch (enemy.reaction)
        {
            case EnemyReaction.IGNORE:
                return;
            case EnemyReaction.PANIC:
                enemy.condition = new Condition(ConditionType.PANIC, 3);
                return;
            case EnemyReaction.RAGE:
                enemy.raged = true;
                return;
            case EnemyReaction.DEAD:
                enemy.HP = 0;
                return;
        }
    }
}