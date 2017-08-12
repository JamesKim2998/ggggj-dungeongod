using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EffectSpawner : MonoBehaviour
{
	private static EffectSpawner _instance;
	private Dictionary<string, List<GameObject>> effects;
	private Dictionary<string, GameObject> originals;

	static EffectSpawner instance
	{
		get
		{
			if (_instance) return _instance;
			var go = new GameObject("Effect Spawner");
			_instance = go.AddComponent<EffectSpawner>();
			return _instance;
		}
	}

	public static GameObject GetEffect(string effectName)
	{
		if (!instance.effects.ContainsKey(effectName))
			instance.effects.Add(effectName, new List<GameObject>());
		var idleEffect = instance.effects[effectName].FirstOrDefault(g => !g.activeSelf);
		if (idleEffect)
			return idleEffect;
		var instanciated = InstantiateEffect(effectName);
		instance.effects[effectName].Add(instanciated);
		instanciated.transform.SetParent(instance.transform, false);
		return instanciated;
	}

	public static void SetEffect(string effectName, Vector3 position)
	{
		var effect = GetEffect(effectName);
		effect.transform.position = position;
		effect.SetActive(true);
	}

	static GameObject InstantiateEffect(string effectName)
	{
		if (!instance.originals.ContainsKey(effectName))
		{
			var loaded = Resources.Load<GameObject>("Effects/" + effectName);
			instance.originals.Add(effectName, loaded);
		}
		return Object.Instantiate<GameObject>(instance.originals[effectName]);
	}

}
