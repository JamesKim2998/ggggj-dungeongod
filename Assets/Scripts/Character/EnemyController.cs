using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public DungeonFloor dungeonFloor;
    protected Enemy character;
    protected Hero hero;
    public int detectDistance = 4;

    public virtual void NextTurn()
    {
    }
}