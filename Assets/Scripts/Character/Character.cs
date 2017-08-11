using UnityEngine;

public class Character : MonoBehaviour
{
	public Coord coord
	{
		get { return Coord.Round(transform.position); }
	}

    public float hp;
    public float power;

    public virtual bool IsDead()
    {
        return false;
    }

    public virtual void Move(Dir dir)
    {
    }

    public virtual void Attack(GameObject target)
    {
    }
}