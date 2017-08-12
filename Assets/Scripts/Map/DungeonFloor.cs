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
	List<Enemy> enemies = new List<Enemy>();

	FogOfWar fogOfWarCache;
	public FogOfWar fogOfWar
	{
		get
		{
			if (fogOfWarCache == null)
			{
				fogOfWarCache = InstantiateFogOfWar();
				fogOfWarCache.transform.SetParent(transform, false);
				fogOfWarCache.transform.position = new Vector3(-0.5f, 3.1f, -0.5f);
			}
			return fogOfWarCache;
		}
	}

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

	void OnDisable()
	{
		foreach (var enemy in enemies)
		{
			if (enemy != null)
				Destroy(enemy.gameObject);
		}
		enemies.Clear();
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

	public IEnumerable<Enemy> EachEnemy()
	{
		var tempEnemies = new List<Enemy>(enemies);
		foreach (var enemy in tempEnemies)
		{
			if (enemy == null) continue;
			yield return enemy;
		}
	}

	/*
	public bool CheckDownStair(Coord coord)
	{
		return false;
	}

	public Trigger GetTriggerOnCoord(Coord coord)
	{
		return null;
	}
	*/

	static FogOfWar InstantiateFogOfWar()
	{
		var prefab = Resources.Load<GameObject>("FogOfWar/FogOfWar");
		return Instantiate(prefab).GetComponent<FogOfWar>();
	}
}
