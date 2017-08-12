using System.Collections.Generic;
using UnityEngine;

public static class Range
{
	public static IEnumerable<Coord> Rect(Coord size)
	{
		if (size.x == 0 || size.y == 0)
			yield break;

		for (var x = 0; x != size.x; ++x)
		{
			yield return new Coord(x, 0);
			yield return new Coord(x, size.y - 1);
		}

		for (var y = 0; y != size.y; ++y)
		{
			yield return new Coord(0, y);
			yield return new Coord(size.x - 1, y);
		}
	}

	public static IEnumerable<Coord> Grid(Coord size)
	{
		for (var y = 0; y != size.y; ++y)
			for (var x = 0; x != size.x; ++x)
				yield return new Coord(x, y);
	}

	public static IEnumerable<Coord> Grid(Coord src, Coord dst)
	{
		if (src.x == dst.x || src.y == dst.y)
			yield break;
		for (var y = src.y; y != dst.y; ++y)
			for (var x = src.x; x != dst.x; ++x)
				yield return new Coord(x, y);
	}

	public static IEnumerable<Coord> Grid(CoordRect rect)
	{
		return Grid(rect.bl, rect.tr);
	}

	public static IEnumerable<Coord> Line(Coord a, Coord b)
	{
		yield return new Coord(0, 0);
		/*
		if (a.x == b.x)
		{
			var yMin = Mathf.Min(a.y, b.y);
			var yMax = Mathf.Max(a.y, b.y);
			for (var y = yMin; y != yMax; ++y)
				yield return new Coord(a.x, y);
		}

		var yMin = Mathf.Min(a.y, b.y);
		var yMax = Mathf.Max(a.y, b.y);
		var oldY = a.y;
		for (var y = yMin; y != yMax; ++y)
			var aVec = a.ToVector3();
		var bVec = b.ToVector3();
		*/
	}

	public static IEnumerable<Coord> InsideDistance(int distance)
	{
		for (var y = -distance; y <= distance; ++y)
		{
			var xAbs = distance - Mathf.Abs(y);
			for (var x = -xAbs; x <= xAbs; ++x)
				yield return new Coord(x, y);
		}
	}
}