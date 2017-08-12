using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public DungeonFloor dungeonFloor;
    public Enemy character;
    public Hero hero;
    public int detectDistance;

    public virtual void NextTurn()
    {
    }
}