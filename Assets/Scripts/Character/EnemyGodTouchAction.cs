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
                //TODO : Enemy Behaviour Rule Sets to Panic
                return;
            case EnemyReaction.RAGE:
                //TODO : Enemy's next attack gets stronger
                return;
            case EnemyReaction.DEAD:
                //TODO : Enemy dead
                return;
        }
    }
}