using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public List<DungeonFloor> floors;
    public DungeonFloor currentFloor;
    public int currentFloorIdx;

    public void GoFloorDown()
    {
        // TOOD
        ++currentFloorIdx;
        currentFloor = DungeonFloorFactory.InstantitateFloor(currentFloorIdx);
    }
}