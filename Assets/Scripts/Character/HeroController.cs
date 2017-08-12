using UnityEngine;

public class HeroController : MonoBehaviour
{
	Hero hero;

	void Awake()
	{
		hero = GetComponent<Hero>();
	}

	Dir nextDirForDebug; // TODO: delete me
    public void NextTurn()
    {
		if (hero == null)
		{
			Debug.LogError("Why null? " + name);
			return;
		}

        // TODO: Character 다음 행동.
		hero.TryToMove(nextDirForDebug);
		nextDirForDebug = nextDirForDebug.Clockwise();
    }
}