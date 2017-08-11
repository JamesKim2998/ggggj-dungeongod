using System;
using UnityEngine;
public class Enemy : Character
{
    protected override void OnCantMove(GameObject target)
    {
        throw new NotImplementedException();
    }

    public EnemyReaction reaction;
    public bool raged = false;

    public override bool Attack(GameObject target)
    {
        Character targetCharacter = target.GetComponent<Character>();

        if (targetCharacter == null)
        {
            return false;
        }

        if (raged)
        {
            targetCharacter.getDamage((int)(power * 1.2f));
            raged = false;
        }
        else
            targetCharacter.getDamage(power);

        return true;
    }
}
