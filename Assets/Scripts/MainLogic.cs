using UnityEngine;

public class MainLogic : MonoBehaviour
{
    public HeroController characterController;
    public EnemyManager enemyManager;

    public void Update()
    {
        UpdateLighting();
        if (CanTransferToNextTurn())
            TransferToNextTurn();
    }

    public bool CanTransferToNextTurn()
    {
        // TODO
        return false;
    }

    public void TransferToNextTurn()
    {
        characterController.NextTurn();
        enemyManager.NextTurn();
    }

    public void UpdateLighting()
    {
    }
}