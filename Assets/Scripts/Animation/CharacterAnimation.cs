using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation : MonoBehaviour
{
	Character me;
	Animator anim;
	AttackContext attackContext;
	System.Action<Character> attackCallback;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		me = GetComponentInParent<Character>();
		anim = GetComponent<Animator>();
	}

	public void Attack(Character target, string trigger = "Attack", System.Action<Character> callback = null)
	{
		anim.SetTrigger(trigger);
		attackContext = new AttackContext();
		attackContext.target = target;
		if (callback != null) attackCallback = callback;
	}

	public void OnAttackPerformed()
	{
		if (attackContext.target.animation)
			attackContext.target.animation.AttackReaction();
		if (attackCallback != null)
		{
			attackCallback(attackContext.target);
			attackCallback = null;
		}
	}

	public void DestroySelf()
	{
		Destroy(me.gameObject);
	}

	public void SetTrigger(string trigger)
	{
		anim.SetTrigger(trigger);
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
	public Character target;
}
