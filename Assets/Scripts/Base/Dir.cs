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
	public static Coord ToCoord(this Dir thiz)
	{
		switch (thiz)
		{
			case Dir.Up: return Coord.up;
			case Dir.Down: return Coord.down;
			case Dir.Right: return Coord.right;
			case Dir.Left: return Coord.left;
			case Dir.Stay: return Coord.zero;
			default: return Coord.zero;
		}
	}

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

    public static Dir Reverse(this Dir thiz)
    {
        switch (thiz)
        {
            case Dir.Up: return Dir.Down;
            case Dir.Down: return Dir.Up;
            case Dir.Right: return Dir.Left;
            case Dir.Left: return Dir.Right;
            default: return Dir.Stay;
        }
    }

	public static int XZAngleFromUp(this Dir thiz)
	{
		switch (thiz)
		{
			case Dir.Up: return 0;
			case Dir.Down: return 180;
			case Dir.Right: return 90;
			case Dir.Left: return -90;
			case Dir.Stay: return 0;
			default: return 0;
		}
	}
}
