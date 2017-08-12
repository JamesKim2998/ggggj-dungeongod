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

		return currentFloor.entrance.transform.position;
	}

	public Vector3 GoBackToFirstFloor()
	{
		currentFloor.gameObject.SetActive(false);
		currentFloor = floors[0];
		currentFloorIdx = 0;
		currentFloor.gameObject.SetActive(true);
		return currentFloor.entrance.transform.position;
	}
}