using UnityEngine;

public struct Coord
{
	public int x;
	public int y;

	public Coord(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public static Coord operator +(Coord a, Coord b)
	{
		return new Coord(a.x + b.x, a.y + b.y);
	}

	public static Coord Round(Vector3 position)
	{
		var x = Mathf.RoundToInt(position.x);
		var y = Mathf.RoundToInt(position.y);
		return new Coord(x, y);
	}
}