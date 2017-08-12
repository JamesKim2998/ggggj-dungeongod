using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapEditor : EditorWindow
{
	const string pathTilePrefab = "Assets/Models/Dungeon/Floors/Floor 1.obj";
	const string pathWallPrefab = "Assets/Models/Dungeon/Pillar 1.obj";
	const string pathStairPrefab = "Assets/Models/Dungeon/Stair.obj";

	Coord _genTileGridSize;
	Coord _genWallGridSize;
	Coord _genRoomSize;

	[MenuItem("Window/GGGGJ/MapEditor")]
	static void Open()
	{
		var window = EditorWindow.GetWindow(typeof(MapEditor));
		window.name = "MapEditor";
		window.Show();
	}

	void OnGUI()
	{
		CoordField("Tile", ref _genTileGridSize);
		if (GUILayout.Button("Gen Tiles"))
			GenTiles(_genTileGridSize);
		CoordField("Wall", ref _genWallGridSize);
		if (GUILayout.Button("Gen Walls"))
			GenWalls(_genWallGridSize);
		CoordField("Room", ref _genRoomSize);
		if (GUILayout.Button("Gen Room"))
			GenRoom(_genRoomSize);
		if (GUILayout.Button("Gen Up Stair"))
			GenUpStair();
		if (GUILayout.Button("Gen Down Stair"))
			GenDownStair();
	}

	static void CoordField(string name, ref Coord coord)
	{
		coord.x = EditorGUILayout.IntField(name + " Width", coord.x);
		coord.y = EditorGUILayout.IntField(name + " Height", coord.y);
	}

	static void Tag(GameObject go, ObjectType type)
	{
		go.AddComponent<ObjectTag>().type = type;
	}

	static void AddColliderCube(GameObject go)
	{
		var c = go.AddComponent<BoxCollider>();
		c.center = new Vector3(0, 0.5f, 0);
		c.size = Vector3.one;
	}

	static void AddColliderUpCube(GameObject go)
	{
		var c = go.AddComponent<BoxCollider>();
		c.center = new Vector3(0, 1.5f, 0);
		c.size = Vector3.one;
	}

	GameObject InstantiateAsset(string prefabPath)
	{
		var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
		return Instantiate(prefab);
	}

	GameObject GenTiles(Coord size)
	{
		var ret = new GameObject("Tiles");
		foreach (var coord in Range.Grid(size))
		{
			var go = InstantiateAsset(pathTilePrefab);
			Tag(go, ObjectType.TILE);
			go.transform.SetParent(ret.transform, false);
			go.transform.position = coord.ToVector3();
		}
		return ret;
	}

	GameObject GenWalls(Coord size)
	{
		var ret = new GameObject("Walls");
		foreach (var coord in Range.Rect(size))
		{
			var go = InstantiateAsset(pathWallPrefab);
			Tag(go, ObjectType.WALL);
			go.transform.SetParent(ret.transform, false);
			go.transform.position = coord.ToVector3(1);
		}
		return ret;
	}

	GameObject GenRoom(Coord size)
	{
		var ret = new GameObject("Room");
		var tiles = GenTiles(size);
		var walls = GenWalls(size);
		tiles.transform.SetParent(ret.transform, false);
		walls.transform.SetParent(ret.transform, false);
		return ret;
	}

	GameObject GenUpStair()
	{
		var go = InstantiateAsset(pathStairPrefab);
		go.name = ObjectType.UP_STAIR.ToString();
		Tag(go, ObjectType.UP_STAIR);
		return go;
	}

	GameObject GenDownStair()
	{
		var go = InstantiateAsset(pathStairPrefab);
		go.name = ObjectType.DOWN_STAIR.ToString();
		Tag(go, ObjectType.DOWN_STAIR);
		AddColliderUpCube(go);
		return go;
	}
}
