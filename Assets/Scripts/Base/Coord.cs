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

	public override string ToString()
	{
		return x + "_" + y;
	}

	public static readonly Coord zero = new Coord(0, 0);
	public static readonly Coord up = new Coord(0, 1);
	public static readonly Coord down = new Coord(0, -1);
	public static readonly Coord left = new Coord(-1, 0);
	public static readonly Coord right = new Coord(1, 0);

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

public struct CoordRect
{
	public int x;
	public int y;
	public int w;
	public int h;

	public Coord bl { get { return new Coord(x, y); } }
	public Coord tr { get { return new Coord(x + w, y + h); } }

	public CoordRect(int x, int y, int w, int h)
	{
		this.x = x;
		this.y = y;
		this.w = w;
		this.h = h;
	}

	public static bool operator ==(CoordRect a, CoordRect b)
	{
		return a.x == b.x && a.y == b.y
			&& a.w == b.w && a.h == b.h;
	}

	public static bool operator !=(CoordRect a, CoordRect b)
	{
		return !(a == b);
	}
}
