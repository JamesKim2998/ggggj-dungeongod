using UnityEngine;

public struct Coord
{
    public int x;
    public int y;

    public static Coord operator +(Coord a, Coord b)
    {
        return default(Coord);
    }

    public static Coord Round(Vector3 position)
    {
        return default(Coord);
    }

    public static int distance(Coord a, Coord b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}