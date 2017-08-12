using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputController : MonoBehaviour
{
	Character character;

	void Awake()
	{
		character = GetComponent<Character>();
	}

	void Update()
	{
		var dir = InputToDir();
		if (dir != Dir.Stay)
			character.TryToMove(dir);
	}

	Dir InputToDir()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow)) return Dir.Up;
		if (Input.GetKeyDown(KeyCode.DownArrow)) return Dir.Down;
		if (Input.GetKeyDown(KeyCode.RightArrow)) return Dir.Right;
		if (Input.GetKeyDown(KeyCode.LeftArrow)) return Dir.Left;
		return Dir.Stay;
	}
}
