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
                enemy.prePanic = true;
                return;
            case EnemyReaction.RAGE:
                enemy.rage ++;
                return;
            case EnemyReaction.DEAD:
                enemy.Die();
                return;
        }
    }
}