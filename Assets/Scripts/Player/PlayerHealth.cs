using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	[SerializeField] private int maxHealth;
	[SerializeField] private int currentHealth;
	[SerializeField] private string animationTriggerName;

	[SerializeField] private Animator anim;
	[SerializeField] private CapsuleCollider2D col;
	[SerializeField] private HealthBar healthBar;

	public bool isDead = false;

	public void Start()
	{
		SetMaxHealth();
	}

	public virtual void TakeDamage(int damage)
	{
		currentHealth -= damage;
		healthBar.SetHealth(currentHealth);

		if (currentHealth <= 0)
		{
			Die();
		}
	}

	public void Die()
	{
		col.enabled = false;
		isDead		= true;
		anim.SetTrigger(animationTriggerName);
	}

	// player death animation event
	private void StopTimeScale()
	{
		Time.timeScale = 0;
		GameManager.instance.isGameOver = true;
	}

	private void SetMaxHealth()
	{
		currentHealth = maxHealth;
		healthBar.SetMaxHealth(maxHealth);
	}
}
