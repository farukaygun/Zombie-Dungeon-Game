using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class HealthSystem : MonoBehaviour
{
	public Animator 		 anim;
	public int 				 maxHealth = 100;
	public int 				 currentHealth;
	public string 			 animationTriggerName;
	public CapsuleCollider2D col;

	public HealthBar  healthBar;

	public virtual void Start()
	{
		SetMaxHealth();

		anim 		  		 = GetComponent<Animator>();
		animationTriggerName = "die";
		col					 = GetComponent<CapsuleCollider2D>();
	}

	public virtual void TakeDamage(int damage)
	{
		currentHealth -= damage;
		healthBar.SetHealth(currentHealth);
		
		if (currentHealth <= 0)
			Die();
	}

	public virtual void Die()
	{
		anim.SetTrigger(animationTriggerName);
		col.enabled = false;
	}

	public virtual void SetMaxHealth()
	{
		currentHealth = maxHealth;
		healthBar.SetMaxHealth(maxHealth);
	}
}
