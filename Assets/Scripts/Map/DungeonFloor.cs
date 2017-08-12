using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class EnemySpawnInfo
{
	public Transform point;
	public string prefabName;
}

public class DungeonFloor : MonoBehaviour
{
	// public List<GameObject> floor;
	// public List<GameObject> items;
	// public List<GameObject> wall;
	public GameObject entrance;
	public Transform enemySpawnRoot;
	public List<EnemySpawnInfo> enemySpawnInfo = new List<EnemySpawnInfo>();
	public List<Enemy> enemies = new List<Enemy>();

	void OnEnable()
	{
		foreach (var spawnInfo in enemySpawnInfo)
		{
			var point = spawnInfo.point.position;
			var prefab = Resources.Load<GameObject>("Enemies/" + spawnInfo.prefabName);
			var enemy = Instantiate(prefab, point, Quaternion.identity, enemySpawnRoot);
			enemies.Add(enemy.GetComponent<Enemy>());
		}
	}

	RaycastHit[] hitsCache = new RaycastHit[16];
	public bool CheckWallExists(Coord coord)
	{
		var hitCount = Physics.RaycastNonAlloc(coord.ToVector3(10), Vector3.down, hitsCache, 20);
		for (var i = 0; i != hitCount; ++i)
		{
			var hitGO = hitsCache[i].collider.gameObject;
			var objTag = hitGO.GetComponent<ObjectTag>();
			if (objTag != null && objTag.type == ObjectType.WALL)
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
