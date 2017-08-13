using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonFloor : MonoBehaviour
{
	public GameObject entrance;

	public Transform enemySpawnRoot;
	public List<GameObject> enemySpawnInfo = new List<GameObject>();
	public List<GameObject> objectsToReset = new List<GameObject>();
	List<Enemy> enemies = new List<Enemy>();
	Item[] items = new Item[0];

	PathFinder pathFinderCache;
	public PathFinder pathFinder
	{
		get
		{
			if (pathFinderCache == null)
				pathFinderCache = new PathFinder(this);
			return pathFinderCache;
		}
	}

	FogOfWar fogOfWarCache;
	public FogOfWar fogOfWar
	{
		get
		{
			if (fogOfWarCache == null)
			{
				fogOfWarCache = InstantiateFogOfWar();
				fogOfWarCache.transform.SetParent(transform, false);
				fogOfWarCache.transform.position = new Vector3(-0.5f, 1.01f, -0.5f);
			}
			return fogOfWarCache;
		}
	}

	void OnEnable()
	{
		int index = 0;
		foreach (var spawn in enemySpawnRoot)
		{
			if (index == enemySpawnInfo.Count) break;
			var spawnPosition = spawn as Transform;
			var point = spawnPosition.position;
			var prefab = Resources.Load<GameObject>("Enemies/" + enemySpawnInfo[index].name);
			var enemy = Instantiate(prefab, point, Quaternion.identity, enemySpawnRoot);
			enemy.GetComponent<Enemy>().initialCoord = Coord.Round(point);
			enemies.Add(enemy.GetComponent<Enemy>());
			index++;
		}
		foreach (var item in objectsToReset)
		{
			item.SetActive(true);
		}

		items = FindObjectsOfType<Item>();
	}

	void OnDisable()
	{
		foreach (var enemy in enemies)
		{
			if (enemy != null)
				Destroy(enemy.gameObject);
		}
		enemies.Clear();

		fogOfWar.ClearHeroMemorizingVisibility();
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

	public IEnumerable<Enemy> EachEnemyFromNearToFar(Coord distanceFrom)
	{
		return EachEnemy()
			.OrderBy(enemy => Coord.distance(enemy.coord, distanceFrom));
	}

	public IEnumerable<Item> EachItem()
	{
		foreach (var item in items)
		{
			if (item == null) continue;
			yield return item;
		}
	}

	public IEnumerable<Item> EachItemFromNearToFar(Coord distanceFrom)
	{
		return EachItem()
			.OrderBy(item => Coord.distance(item.coord, distanceFrom));
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
