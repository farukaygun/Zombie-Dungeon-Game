using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthSystem : MonoBehaviour
{
	public Animator 		 anim;
	public int 				 maxHealth = 100;
	public int 				 currentHealth;
	public string 			 animationTriggerName;
	public CapsuleCollider2D col;

	public virtual void Start()
	{
		currentHealth 		 = maxHealth;
		anim 		  		 = GetComponent<Animator>();
		animationTriggerName = "die";
		col					 = GetComponent<CapsuleCollider2D>();
	}

	public virtual void TakeDamage(int damage)
	{
		currentHealth -= damage;
		
		if (currentHealth <= 0)
			Die();
	}

	public virtual void Die()
	{
		anim.SetTrigger(animationTriggerName);
		col.enabled = false;
	}
}
