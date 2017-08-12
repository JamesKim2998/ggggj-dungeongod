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
    public LayerMask blockingLayer; // set to collidable layer

    private BoxCollider boxCollider; //we need box collider
    private Rigidbody rigidbody;     //we need rigidbody. turn off gravity.
    private float inverseMoveTime;

    public Condition condition;

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        rigidbody = GetComponent<Rigidbody>();
        inverseMoveTime = 1f / moveTime;
    }

    //return if Moving was successful
    public virtual bool Move(Dir dir, out RaycastHit hitInfo)
    {
        Vector3 start = transform.position;
        Vector3 dest = start + dir.ToVector3();
        bool isHit = false;

        boxCollider.enabled = false;
        isHit = Physics.Linecast(start, dest, out hitInfo, blockingLayer);
        boxCollider.enabled = true;

        if(!isHit)
        {
			transform.position = dest;
			// TODO: 조작감 문제로 일단 주석처리.
            // StartCoroutine(SmoothMovement(dest));

            return true;
        }

        return false;

    }

    // changing location
    protected IEnumerator SmoothMovement(Vector3 dest)
    {
        float sqrRemainingDistance = (transform.position - dest).sqrMagnitude;
        
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPostion = Vector3.MoveTowards(rigidbody.position, dest, inverseMoveTime * Time.deltaTime);
            
            rigidbody.MovePosition(newPostion);
            
            sqrRemainingDistance = (transform.position - dest).sqrMagnitude;
            
            yield return null;
        }
    }

    /*protected*/ public /**/ virtual void TryToMove ( Dir dir)
    {
        RaycastHit hitInfo;

        bool canMove = Move(dir, out hitInfo);

        if(canMove)
        {
            return;
        }
        
        OnCantMove(hitInfo.transform.gameObject);
    }

    public abstract void OnCantMove(GameObject target);

    public void getDamage(int value)
    {
        this.HP -= value;

        if (IsDead())
        {
            Die();
        }
    }

    public virtual void Heal(int value)
    {
        this.HP = Mathf.Clamp(this.HP + value, 0, this.maxHP);
    }


    public bool IsDead()
    {
        if(this.HP <= 0)
        {
            return true;
        }
        return false;
    }


    public abstract void Die();
}