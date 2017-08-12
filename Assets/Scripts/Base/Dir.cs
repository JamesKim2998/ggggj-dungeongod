using UnityEngine;

public enum Dir
{
	Up,
	Down,
	Right,
	Left,
	Stay,
}

public static partial class ExtensionMethods
{
	public static Vector3 ToVector3(this Dir thiz)
	{
		switch (thiz)
		{
			case Dir.Up: return Vector3.forward;
			case Dir.Down: return Vector3.back;
			case Dir.Right: return Vector3.right;
			case Dir.Left: return Vector3.left;
			case Dir.Stay: return Vector3.zero;
			default: return Vector3.zero;
		}
	}

	public static Dir Clockwise(this Dir thiz)
	{
		switch (thiz)
		{
			case Dir.Up: return Dir.Right;
			case Dir.Down: return Dir.Left;
			case Dir.Right: return Dir.Down;
			case Dir.Left: return Dir.Up;
			default: return Dir.Stay;
		}
	}
}
