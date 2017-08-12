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
        // TODO: Character 다음 행동.
		hero.TryToMove(nextDirForDebug);
		nextDirForDebug = nextDirForDebug.Clockwise();
    }
}