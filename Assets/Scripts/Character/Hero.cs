using System;
using UnityEngine;

public class Hero : Character
{
    public bool buffed = false;
    public int buffedTurn;

    public void checkBuffEnded()
    {
        if (buffedTurn <= 0)
            buffed = false;
    }

    protected override void OnCantMove(GameObject target)
    {
        throw new NotImplementedException();
    }
}