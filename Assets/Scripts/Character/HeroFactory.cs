using UnityEngine;

public static class HeroFactory
{
	static GameObject[] heads;
	static GameObject[] bodies;
	static Color GetRandomColor()
	{
		return Color.HSVToRGB(Random.Range(0f, 1f), Random.Range(0f, 0.8f), Random.Range(0f, 1f));
	}
	static GameObject GetRandomHead()
	{
		if (heads == null)
		{
			heads = Resources.LoadAll<GameObject>("Hero/Head");
		}
		return heads[Random.Range(0, heads.Length)];
	}

	static GameObject GetRandomBody()
	{
		if (bodies == null)
		{
			bodies = Resources.LoadAll<GameObject>("Hero/Body");
		}
		return bodies[Random.Range(0, bodies.Length)];
	}

	public static Hero InstantiateRandom()
	{
		var prefab = Resources.Load<GameObject>("Hero");
		var hero = Object.Instantiate(prefab).GetComponent<Hero>();
		hero.transform.position = Vector3.one;
		var children = hero.transform.GetComponentsInChildren<Transform>();
		foreach (Transform child in children)
			Object.Destroy(child.gameObject);
		var body = Object.Instantiate<GameObject>(GetRandomBody());
		var head = Object.Instantiate<GameObject>(GetRandomHead());
		body.transform.SetParent(hero.transform, false);
		head.transform.SetParent(hero.transform, false);
		Shader.SetGlobalColor("_HairColor", GetRandomColor());
		Shader.SetGlobalColor("_BodyColor", GetRandomColor());

		return hero;
	}
}
