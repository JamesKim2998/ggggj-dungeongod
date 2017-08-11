using System;
using UnityEngine;
public class Enemy : Character
{
    protected override void OnCantMove(GameObject target)
    {
        throw new NotImplementedException();
    }

    public EnemyReaction reaction;
}
