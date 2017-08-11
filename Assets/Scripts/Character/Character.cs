using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour
{
	public Coord coord
	{
		get { return Coord.Round(transform.position); }
	}

    public int maxHP = 15;
    public int HP = 10;
    public int power = 5;

    public float moveTime = 1f;
    public LayerMask blockingLayer;

    private BoxCollider boxCollider;

    public Condition condition;

    public static Vector3 dirToVector3(Dir dir)
    {
        switch(dir)
        {
            case Dir.Up: return new Vector3(0, 0, 1);
            case Dir.Down: return new Vector3(0, 0, -1);
            case Dir.Right: return new Vector3(1, 0, 0);
            case Dir.Left: return new Vector3(-1, 0, 0);
            default: return new Vector3();
        }
    }

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    //return if Moving was successful
    public virtual bool Move(Dir dir, out RaycastHit hitInfo)
    {
        Vector3 start = transform.position;
        Vector3 dest = start + Character.dirToVector3(dir);
        bool isHit = false;

        boxCollider.enabled = false;
        isHit = Physics.Linecast(start, dest, out hitInfo, blockingLayer);
        boxCollider.enabled = true;

        if(!isHit)
        {
            //Todo Move to destination

            return true;
        }

        return false;

    }

    protected virtual void TryToMove ( Dir dir)
    {
        RaycastHit hitInfo;

        bool canMove = Move(dir, out hitInfo);

        if(canMove)
        {
            return;
        }

        OnCantMove(hitInfo.transform.gameObject);
    }

    protected abstract void OnCantMove(GameObject target);
    

    public virtual bool Attack(GameObject target)
    {
        Character targetCharacter = target.GetComponent<Character>();
        
        if (targetCharacter == null)
        {
            return false;
        }

        targetCharacter.getDamage(power);

        return true;
    }

    public virtual void Heal(int heal)
    {
        this.HP = Mathf.Clamp(this.HP + heal, 0, this.maxHP);
    }

    public virtual void getDamage(int damage)
    {
        this.HP -= damage;
    }

    public virtual bool IsDead()
    {
        if(this.HP <= 0)
        {
            return true;
        }
        return false;
    }
}
