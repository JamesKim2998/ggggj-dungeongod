using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation : MonoBehaviour
{
	Character me;
	Animator anim;
	AttackContext attackContext;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		me = GetComponentInParent<Character>();
		anim = GetComponent<Animator>();
	}

	public void Attack(CharacterAnimation target, bool successful, string trigger = "Attack")
	{
		anim.SetTrigger(trigger);
		attackContext = new AttackContext();
		attackContext.target = target;
		attackContext.successful = successful;
	}

	public void OnAttackPerformed()
	{
		attackContext.target.AttackReaction();
		if (attackContext.successful)
			EffectSpawner.SetEffect("HitEffect", attackContext.target.transform.position + Vector3.up);
		else
			EffectSpawner.SetEffect("MISS", attackContext.target.transform.position + Vector3.up);
	}

	public void DestroySelf()
	{
		Destroy(me.gameObject);
	}

	public void SetTrigger(string trigger)
	{
		anim.SetTrigger(trigger);
	}

	public void Move()
	{
		anim.SetBool("Moving", true);
	}

	public void OnDieAnimationEnd()
	{
		me.Die();
	}

	void AttackReaction()
	{
		if (me.IsDead())
		{
			anim.SetTrigger("Die");
		}
		else
		{
			anim.SetTrigger("Hit");
		}
	}
}

public struct AttackContext
{
	public CharacterAnimation target;
	public bool successful;
}
