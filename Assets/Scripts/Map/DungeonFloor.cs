using System.Collections.Generic;
using UnityEngine;

public class DungeonFloor : MonoBehaviour
{
	// public List<GameObject> floor;
	// public List<GameObject> items;
	// public List<GameObject> wall;
	public List<Enemy> enemies = new List<Enemy>();

	RaycastHit[] hitsCache = new RaycastHit[16];
	public bool CheckWallExists(Coord coord)
	{
		var hitCount = Physics.RaycastNonAlloc(coord.ToVector3(10), Vector3.down, hitsCache, 20);
		for (var i = 0; i != hitCount; ++i)
		{
			var hitGO = hitsCache[i].collider.gameObject;
			var objTag = hitGO.GetComponent<MapEditorObjectTag>();
			if (objTag != null && objTag.type == MapEditorObjectType.WALL)
				return true;
		}
		return false;
	}

	public bool CheckDownStair(Coord coord)
	{
		return false;
	}

	public Trigger GetTriggerOnCoord(Coord coord)
	{
		return null;
	}
}
