using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
	TILE,
	WALL,
	UP_STAIR,
	DOWN_STAIR,
}

public class ObjectTag : MonoBehaviour
{
	public ObjectType type;
}
