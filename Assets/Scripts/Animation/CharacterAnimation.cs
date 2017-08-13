using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation : MonoBehaviour
{
	Character me;
	Animator anim;
	Character target;
	System.Action<Character> attackCallback;
	bool duringDeathAnimation = false;

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
		if (me.IsDead()) return;
		anim.SetTrigger(trigger);
		this.target = target;
		if (callback != null) attackCallback = callback;
	}

	public void OnAttackPerformed()
	{
		if (target == null) return;
		if (target.animation)
			target.animation.AttackReaction();
		if (attackCallback != null)
		{
			attackCallback(target);
			attackCallback = null;
		}
	}

	public void DestroySelf()
	{
		Destroy(me.gameObject);
	}

	public void SetTrigger(string trigger)
	{
		if (me.IsDead() && trigger != "Die") return;
		if (trigger == "Die")
		{
			if (duringDeathAnimation) return;
			duringDeathAnimation = true;
		}
		anim.SetTrigger(trigger);
	}

	public void OnDieAnimationEnd()
	{
		me.Die();
	}

	public void OnJumpedDown()
	{
		(me as Hero).JumpedDown();
	}

	void AttackReaction()
	{
	}
}