using UnityEngine;

public static class HeroFactory
{
	public static Hero InstantiateRandom()
	{
		var prefab = Resources.Load<GameObject>("Hero");
		var hero = Object.Instantiate(prefab).GetComponent<Hero>();
		hero.transform.position = Vector3.one;
		return hero;
	}
}
