using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
	public GameObject prefabQuad;
	public Transform quadRoot;
	public Material matGodVisible;

	class QuadInfo
	{
		public bool godVisible;
		public GameObject go;
		public MeshRenderer renderer;
	}

	readonly Dictionary<Coord, QuadInfo> quads = new Dictionary<Coord, QuadInfo>();
	HashSet<Coord> heroVisiblity = new HashSet<Coord>();
	CoordRect oldBoundingRect;

	void Update()
	{
		UpdateFrustrum();
	}

	QuadInfo PullQuad(Coord coord)
	{
		QuadInfo quadInfo;
		if (quads.TryGetValue(coord, out quadInfo))
			return quadInfo;

		quadInfo = new QuadInfo();
		quads[coord] = quadInfo;

		var quad = Instantiate(prefabQuad, coord.ToVector3(), Quaternion.identity, quadRoot);
		quad.name = coord.ToString();
		quad.transform.eulerAngles = new Vector3(90, 0, 0);
		quad.SetActive(true);

		quadInfo.go = quad;
		quadInfo.renderer = quad.GetComponent<MeshRenderer>();

		return quadInfo;
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
			PullQuad(coord);
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

	void SetHeroVisible(Coord coord)
	{
		var quadInfo = PullQuad(coord);
		quadInfo.godVisible = true;
		if (quadInfo.go.activeSelf)
			quadInfo.go.SetActive(false);
	}

	void SetGodVisible(Coord coord)
	{
		var quadInfo = PullQuad(coord);
		quadInfo.godVisible = true;
		if (!quadInfo.go.activeSelf)
			quadInfo.go.SetActive(true);
		quadInfo.renderer.sharedMaterial = matGodVisible;
	}

	public void UpdateVisibilty(Character character, int visibleDistance)
	{
		var center = character.coord;

		var oldVisible = heroVisiblity;
		heroVisiblity = new HashSet<Coord>();

		// TODO: 벽체크.
		foreach (var testDelta in Range.InsideDistance(visibleDistance))
		{
			var testCoord = center + testDelta;
			if (!character.CanSee(testCoord)) continue;
			SetHeroVisible(testCoord);
			heroVisiblity.Add(testCoord);
		}

		foreach (var oldVisibleCoord in oldVisible)
		{
			if (!heroVisiblity.Contains(oldVisibleCoord))
				SetGodVisible(oldVisibleCoord);
		}
	}
}