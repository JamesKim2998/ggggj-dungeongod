using System.Collections.Generic;

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
		for (var x = 0; x != size.x; ++x)
			for (var y = 0; y != size.y; ++y)
				yield return new Coord(x, y);
	}
}