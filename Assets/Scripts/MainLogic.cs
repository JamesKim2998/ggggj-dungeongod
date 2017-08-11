using UnityEngine;

public class MainLogic : MonoBehaviour
{
    public Dungeon dungeon;
    public God god;
    public Hero hero;
    public HeroController characterController;
    public EnemyManager enemyManager;

    public void Update()
    {
        TryFloorDown();
        god.Update();
        UpdateGodTouch();
        if (CanTransferToNextTurn())
            TransferToNextTurn();
    }

    public void TryFloorDown()
    {
        var floor = dungeon.currentFloor;
        if (floor.CheckDownStair(hero.coord))
            dungeon.GoFloorDown();
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

    // TODO
    public RaycastHit[] TestGodTouch()
    {
        return null;
    }

    public void UpdateGodTouch()
    {
        var hits = TestGodTouch();
        foreach (var hit in hits)
        {
            var hitGO = hit.collider.gameObject;
            var godTouch = hitGO.GetComponent<GodTouchAction>();
            if (godTouch != null) godTouch.Act();
        }
    }
}