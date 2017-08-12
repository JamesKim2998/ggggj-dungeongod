using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
	public List<DungeonFloor> floors;
	public DungeonFloor currentFloor;
	public int currentFloorIdx;

	public void Clear()
	{
		foreach (var floor in floors)
			currentFloor.gameObject.SetActive(false);
		currentFloorIdx = 0;
	}

	public Vector3 LoadInitLevel()
	{
		currentFloorIdx = -1;
		return GoToNextFloor();
	}

	public Vector3 GoToNextFloor()
	{
		currentFloor.gameObject.SetActive(false);

		var nextFloor = ++currentFloorIdx;
		if (nextFloor < floors.Count)
		{
			currentFloor = floors[nextFloor];
			currentFloor.gameObject.SetActive(true);
		}
		else
		{
			currentFloor = DungeonFloorFactory.InstantitateFloor(nextFloor);
			floors.Add(currentFloor);
		}

        MainLogic.instance.pathFinder.UpdateMapInfo(currentFloor);

		return currentFloor.entrance.transform.position;
	}
}