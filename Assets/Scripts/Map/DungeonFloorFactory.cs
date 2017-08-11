using UnityEngine;

public static class DungeonFloorFactory
{
    public static DungeonFloor InstantitateFloor(int floorToInstantiate)
	{
		return InstantitateByPrefab(floorToInstantiate);
	}

    static DungeonFloor InstantitateByPrefab(int floorToInstantiate)
    {
        // TODO: 우선순위 낮음.
        // Resources.Load()
        return null;
    }

    static DungeonFloor InstantitateByRandom()
    {
        // TODO: 우선순위 낮음.
        return null;
    }
}