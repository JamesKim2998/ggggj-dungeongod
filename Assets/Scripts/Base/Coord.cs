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

	public Vector3 ToVector3(float y = 0)
	{
		return new Vector3(this.x, y, this.y);
	}

	public static Coord operator +(Coord a, Coord b)
	{
		return new Coord(a.x + b.x, a.y + b.y);
	}

	public static Coord Round(Vector3 position)
	{
		var x = Mathf.RoundToInt(position.x);
		var y = Mathf.RoundToInt(position.z);
		return new Coord(x, y);
	}

    public static int distance(Coord a, Coord b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}
