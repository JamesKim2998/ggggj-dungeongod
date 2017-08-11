using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapEditorObjectType
{
	TILE,
	WALL,
}

public class MapEditorObjectTag : MonoBehaviour
{
	public MapEditorObjectType type;
}
