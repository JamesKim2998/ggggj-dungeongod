using UnityEngine;

public static class DungeonFloorFactory
{
	public static DungeonFloor InstantitateFloor(int floorToInstantiate)
	{
		return InstantitateByPrefab(floorToInstantiate);
	}

	static DungeonFloor InstantitateByPrefab(int floorToInstantiate)
	{
		var floor = Mathf.Clamp(floorToInstantiate, 1, 5);
		var floorPrefab = Resources.Load<GameObject>("Levels/Level" + floor);
		return Object.Instantiate(floorPrefab).GetComponent<DungeonFloor>();
	}

	static DungeonFloor InstantitateByRandom()
	{
		// TODO: 우선순위 낮음.
		return null;
	}
}