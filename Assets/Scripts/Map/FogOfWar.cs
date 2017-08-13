using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
	enum Visiblity
	{
		None,
		God,
		Hero,
	}

	struct QuadInfo
	{
		public int quadIdx;
		public Visiblity visiblity;
	}

	public MeshFilter meshFilter;
	Mesh mesh;
	CoordRect oldBoundingRect;

	HashSet<Coord> visibleToHero = new HashSet<Coord>();
	readonly Dictionary<Coord, QuadInfo> quadInfos = new Dictionary<Coord, QuadInfo>();
	readonly Dictionary<Coord, Visiblity> quadUpdates = new Dictionary<Coord, Visiblity>();

	public HashSet<Coord> heroMemorizingVisiblity = new HashSet<Coord>();

	void Awake()
	{
		mesh = new Mesh();
		meshFilter.mesh = mesh;
	}

	void RequestUpdate(Coord coord, Visiblity visiblity)
	{
		quadUpdates[coord] = visiblity;
	}

	void RequestAdd(Coord coord)
	{
		if (quadInfos.ContainsKey(coord)) return;
		if (quadUpdates.ContainsKey(coord)) return;
		quadUpdates[coord] = Visiblity.None;
	}

	public bool IsVisibleByGod(Coord coord)
	{
		QuadInfo test;
		if (!quadInfos.TryGetValue(coord, out test))
			return false;
		return test.visiblity != Visiblity.None;
	}

	public bool IsHeroMemorizeAsVisible(Coord coord)
	{
		return heroMemorizingVisiblity.Contains(coord);
	}

	void ResolveAllUpdateVisiblityRequest()
	{
		if (quadUpdates.Count == 0)
			return;

		var addedQuadCount = 0;
		foreach (var update in quadUpdates)
		{
			if (!quadInfos.ContainsKey(update.Key))
				++addedQuadCount;
		}

		var newQuadCount = quadInfos.Count + addedQuadCount;
		var newVertexCount = newQuadCount * 4;
		var newUVCount = newVertexCount;
		var newTriangleCount = newQuadCount * 6;
		var newVertexes = new Vector3[newVertexCount];
		var newUVs = new Vector2[newUVCount];
		var newTriangles = new int[newTriangleCount];
		System.Array.Copy(mesh.vertices, newVertexes, mesh.vertices.Length);
		System.Array.Copy(mesh.uv, newUVs, mesh.uv.Length);
		System.Array.Copy(mesh.triangles, newTriangles, mesh.triangles.Length);

		foreach (var update in quadUpdates)
		{
			ResolveUpdateVisiblityRequest(
				update.Key, update.Value,
				newVertexes, newUVs, newTriangles);
		}
		mesh.vertices = newVertexes;
		mesh.uv = newUVs;
		mesh.triangles = newTriangles;
		mesh.UploadMeshData(false);

		quadUpdates.Clear();
	}

	void ResolveUpdateVisiblityRequest(
		Coord coord, Visiblity visiblity,
		Vector3[] vertexes, Vector2[] uvs, int[] triangles)
	{
		QuadInfo quadInfo;
		if (!quadInfos.TryGetValue(coord, out quadInfo))
		{
			quadInfo.quadIdx = quadInfos.Count;
			quadInfo.visiblity = visiblity;
			quadInfos.Add(coord, quadInfo);

			var quadIdx = quadInfo.quadIdx;
			var vStart = quadIdx * 4;
			var origin = coord.ToVector3();
			vertexes[vStart] = origin;
			vertexes[vStart + 1] = origin + Vector3.forward;
			vertexes[vStart + 2] = origin + new Vector3(1, 0, 1);
			vertexes[vStart + 3] = origin + Vector3.right;
			var tStart = quadIdx * 6;
			triangles[tStart] = vStart;
			triangles[tStart + 1] = vStart + 1;
			triangles[tStart + 2] = vStart + 2;
			triangles[tStart + 3] = vStart;
			triangles[tStart + 4] = vStart + 2;
			triangles[tStart + 5] = vStart + 3;
		}
		else
		{
			var oldVisiblity = quadInfo.visiblity;
			if (oldVisiblity == visiblity) return;
			quadInfo.visiblity = visiblity;
		}

		quadInfos[coord] = quadInfo;
		var uvStart = quadInfo.quadIdx * 4;
		var alpha = AlphaForVisiblity(visiblity);
		var alpha2 = new Vector2(alpha, alpha);
		uvs[uvStart] = alpha2;
		uvs[uvStart + 1] = alpha2;
		uvs[uvStart + 2] = alpha2;
		uvs[uvStart + 3] = alpha2;
	}

	static float AlphaForVisiblity(Visiblity visiblity)
	{
		switch (visiblity)
		{
			case Visiblity.None: return 1;
			case Visiblity.God: return 0.8f;
			case Visiblity.Hero: return 0;
			default: return 1;
		}
	}

	void Update()
	{
		UpdateFrustrum();
		ResolveAllUpdateVisiblityRequest();
	}

	void UpdateFrustrum()
	{
		var bl = ViewportToXZ(Vector3.zero);
		var br = ViewportToXZ(Vector3.right);
		var tl = ViewportToXZ(Vector3.up);
		var tr = ViewportToXZ(Vector3.one);

		var boundingRect = BoundingXZRect(bl, br, tl, tr);
		boundingRect.x -= 1;
		boundingRect.y -= 1;
		boundingRect.w += 2;
		boundingRect.h += 2;

		if (oldBoundingRect == boundingRect) return;
		oldBoundingRect = boundingRect;

		foreach (var coord in Range.Grid(boundingRect))
			RequestAdd(coord);
	}

	public static CoordRect BoundingXZRect(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		var xMin = Mathf.FloorToInt(Mathf.Min(a.x, b.x, c.x, d.x));
		var xMax = Mathf.CeilToInt(Mathf.Max(a.x, b.x, c.x, d.x));
		var zMin = Mathf.FloorToInt(Mathf.Min(a.z, b.z, c.z, d.z));
		var zMax = Mathf.CeilToInt(Mathf.Max(a.z, b.z, c.z, d.z));
		return new CoordRect(xMin, zMin, xMax - xMin, zMax - zMin);
	}

	static Vector3 ViewportToXZ(Vector2 viewportPoint)
	{
		var cam = Camera.main;
		var ray = cam.ViewportPointToRay(viewportPoint);
		return ray.origin - ray.direction * (ray.origin.y / ray.direction.y);
	}

	public void UpdateVisibilty(Character character, int visibleDistance)
	{
		var center = character.coord;

		var oldVisibleToVisible = visibleToHero;
		visibleToHero = new HashSet<Coord>();

		foreach (var testDelta in Range.InsideDistance(visibleDistance))
		{
			var testCoord = center + testDelta;

			// Check wall
			RaycastHit hitInfo;
			if (!character.CanSee(testCoord, out hitInfo))
				continue;

			RequestUpdate(testCoord, Visiblity.Hero);
			visibleToHero.Add(testCoord);
			heroMemorizingVisiblity.Add(testCoord);
		}

		foreach (var oldVisibleCoord in oldVisibleToVisible)
		{
			if (!visibleToHero.Contains(oldVisibleCoord))
				RequestUpdate(oldVisibleCoord, Visiblity.God);
		}
	}

	public void ClearHeroMemorizingVisibility()
	{
		heroMemorizingVisiblity.Clear();
	}
}