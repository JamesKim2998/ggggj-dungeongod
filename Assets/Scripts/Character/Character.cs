using UnityEngine;
using System.Collections;
using DG.Tweening;

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

	public ConditionType condition;
	public int visibleDistance = 10;
	public new CharacterAnimation animation;

	protected virtual void Awake()
	{
		boxCollider = GetComponent<BoxCollider>();
		rigidbody = GetComponent<Rigidbody>();
		animation = GetComponentInChildren<CharacterAnimation>();
		inverseMoveTime = 1f / moveTime;
	}

	public bool IsCoordBlocked(Coord testCoord, out RaycastHit hitInfo)
	{
		var start = transform.position;
		var dest = testCoord.ToVector3(start.y);
		bool isHit = false;
		boxCollider.enabled = false;
		isHit = Physics.Linecast(start, dest,
			out hitInfo, blockingLayer);
		boxCollider.enabled = true;
		return isHit;
	}

	public bool CanSee(Coord testCoord, out RaycastHit hitInfo)
	{
		if (Coord.distance(testCoord, coord) > visibleDistance)
		{
			hitInfo = default(RaycastHit);
			return false;
		}
		var isBlocked = IsCoordBlocked(testCoord, out hitInfo);
		if (!isBlocked) return true;
		return Coord.Round(hitInfo.collider.transform.position) == testCoord;
	}

	public bool CanSee(Coord testCoord)
	{
		RaycastHit unused;
		return CanSee(testCoord, out unused);
	}

	//return if Moving was successful
	public virtual bool Move(Dir dir, out RaycastHit hitInfo)
	{
		var newEulerAngles = transform.eulerAngles;
		newEulerAngles.y = dir.XZAngleFromUp();
		transform.eulerAngles = newEulerAngles;

		var dest = coord + dir.ToCoord();
		var isHit = IsCoordBlocked(dest, out hitInfo);

		if (!isHit)
		{
			var oldPos = transform.position;
			transform.DOMove(dest.ToVector3(oldPos.y), 0.5f);
			if (animation) animation.SetTrigger("Move");
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

	/*protected*/
	public /**/ virtual void TryToMove(Dir dir)
	{
		RaycastHit hitInfo;

		bool canMove = Move(dir, out hitInfo);

		if (canMove)
		{
			return;
		}

		OnCantMove(hitInfo.transform.gameObject);
	}

	public abstract void OnCantMove(GameObject target);

	public int DiceRoll()
	{
		return Random.Range(1, 7) + Random.Range(1, 7);
	}

	public virtual void getDamage(int power, int dice)
	{
		int powerDiff = power + dice - this.power;
		bool hit = false;
		if (powerDiff >= 12)
		{
			//CRITICAL !! TODO ANIMATION
			this.HP -= 2;
			EffectSpawner.SetEffect("HitEffect", transform.position + Vector3.up);
			hit = true;
		}
		else if (powerDiff >= 7 || dice >= 12)
		{
			//normal hit;
			this.HP -= 1;
			EffectSpawner.SetEffect("HitEffect", transform.position + Vector3.up);
			hit = true;
		}
		else
		{
			//MISS!!
			EffectSpawner.SetEffect("MISS", transform.position + Vector3.up);
		}

		if (IsDead())
		{
			if (animation) animation.SetTrigger("Die");
		}
		else
		{
			if (animation && hit) animation.SetTrigger("Hit");
		}
	}

	public virtual void Heal(int value)
	{
		this.HP = Mathf.Clamp(this.HP + value, 0, this.maxHP);
	}


	public bool IsDead()
	{
		if (this.HP <= 0)
		{
			return true;
		}
		return false;
	}


	public abstract void Die();
}
