using UnityEngine;

public static class DungeonFloorFactory
{
	public static DungeonFloor InstantitateFloor(int floorToInstantiate)
	{
		return InstantitateByPrefab(floorToInstantiate);
	}

	static DungeonFloor InstantitateByPrefab(int floorToInstantiate)
	{
		var floorPrefab = Resources.Load<GameObject>("Floors/Floor0");
		return Object.Instantiate(floorPrefab).GetComponent<DungeonFloor>();
	}

	static DungeonFloor InstantitateByRandom()
	{
		// TODO: 우선순위 낮음.
		return null;
	}
}