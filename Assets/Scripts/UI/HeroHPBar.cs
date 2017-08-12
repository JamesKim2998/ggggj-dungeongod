using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroHPBar : MonoBehaviour
{
	public GameObject heartPrefab;
	public Transform heartRoot;
	public Sprite spriteFull;
	public Sprite spriteBlank;

	readonly List<GameObject> hearts = new List<GameObject>();
	int curHP;


	void Awake()
	{
		// StartCoroutine(CoroutineTest());
	}

	public void SetMaxHP(int maxHP, bool fullHearts)
	{
		var oldMaxHP = hearts.Count;

		if (oldMaxHP < maxHP)
		{
			for (var i = oldMaxHP; i != maxHP; ++i)
			{
				var heart = Instantiate(heartPrefab, heartRoot);
				heart.SetActive(true);
				hearts.Add(heart);
			}
		}
		else if (oldMaxHP > maxHP)
		{
			for (var i = maxHP; i != oldMaxHP; ++i)
				Destroy(hearts[i]);
		}

		if (fullHearts)
		{
			curHP = maxHP;
			foreach (var heart in hearts)
				SetFullHeart(heart);
		}
		else
		{
			if (curHP > maxHP)
				curHP = maxHP;
		}
	}

	void SetFullHeart(GameObject heart)
	{
		var image = heart.GetComponent<Image>();
		image.sprite = spriteFull;
	}

	void SetBlankHeart(GameObject heart)
	{
		var image = heart.GetComponent<Image>();
		image.sprite = spriteBlank;
	}

	public void SetHP(int hp)
	{
		hp = Mathf.Clamp(hp, 0, hearts.Count + 1);
		if (hp == curHP) return;

		if (hp > curHP)
		{
			for (var i = curHP; i < hp; ++i)
				SetFullHeart(hearts[i]);
		}
		else
		{
			for (var i = hp; i < curHP; ++i)
				SetBlankHeart(hearts[i]);
		}

		curHP = hp;
	}

	IEnumerator CoroutineTest()
	{
		for (var i = 0; i != 20; ++i)
		{
			SetMaxHP(i, true);
			yield return new WaitForSeconds(0.1f);
			for (var j = 0; j != i; ++j)
			{
				SetHP(j);
				yield return new WaitForSeconds(0.1f);
			}
		}
	}
}
