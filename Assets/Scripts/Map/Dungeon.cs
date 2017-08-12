using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public List<DungeonFloor> floors;
    public DungeonFloor currentFloor;
    public int currentFloorIdx;

	public void Clear()
	{
		// TODO
	}

	public void LoadInitLevel()
	{
		DungeonFloorFactory.InstantitateFloor(0);
	}

    public void GoToNextFloor()
    {
        // TOOD
        ++currentFloorIdx;
        currentFloor = DungeonFloorFactory.InstantitateFloor(currentFloorIdx);
    }
}