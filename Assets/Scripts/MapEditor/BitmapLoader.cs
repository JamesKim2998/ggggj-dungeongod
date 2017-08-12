using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BitmapLoader : MonoBehaviour
{
	public Texture2D texture;
	public GameObject floor;
	public GameObject wall;
	public Mesh[] floorModels;
	public Mesh[] wallModels;
	public void LoadBitmap()
	{
		HashSet<Coord> coords = new HashSet<Coord>();
		HashSet<Coord> wallCoords = new HashSet<Coord>();
		for (int x = 0; x < texture.width; x++)
		{
			for (int y = 0; y < texture.height; y++)
			{
				var color = texture.GetPixel(x, y);
				if (color.a > 0.8f) coords.Add(new Coord(x, y));
			}
		}
		var newMapGO = new GameObject("Level");
		var newMap = newMapGO.transform;
		foreach (var coord in coords)
		{
			SpawnFloor(coord.x, coord.y, newMap);
			SpawnWall(coords, wallCoords, coord.x + 1, coord.y, newMap);
			SpawnWall(coords, wallCoords, coord.x, coord.y + 1, newMap);
			SpawnWall(coords, wallCoords, coord.x - 1, coord.y, newMap);
			SpawnWall(coords, wallCoords, coord.x, coord.y - 1, newMap);
			SpawnWall(coords, wallCoords, coord.x + 1, coord.y + 1, newMap);
			SpawnWall(coords, wallCoords, coord.x + 1, coord.y - 1, newMap);
			SpawnWall(coords, wallCoords, coord.x - 1, coord.y + 1, newMap);
			SpawnWall(coords, wallCoords, coord.x - 1, coord.y - 1, newMap);
		}
	}
	void SpawnFloor(int x, int y, Transform newMap)
	{
		var spawned = Instantiate<GameObject>(floor);
		spawned.transform.SetParent(newMap);
		spawned.transform.position = new Vector3(x, -1, y);
		spawned.GetComponentInChildren<MeshFilter>().mesh = floorModels[Random.Range(0, floorModels.Length)];
	}
	void SpawnWall(HashSet<Coord> coordSet, HashSet<Coord> walls, int x, int y, Transform newMap)
	{
		if (coordSet.Any(c => c.x == x && c.y == y) || walls.Any(c => c.x == x && c.y == y)) return;
		walls.Add(new Coord(x, y));
		var spawned = Instantiate<GameObject>(wall);
		spawned.transform.SetParent(newMap);
		spawned.transform.position = new Vector3(x, -1, y);
		spawned.transform.localRotation = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
		spawned.GetComponentInChildren<MeshFilter>().mesh = wallModels[Random.Range(0, wallModels.Length)];
	}
}