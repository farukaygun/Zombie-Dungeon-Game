using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	[Header("Health System")]
	private int	   maxHealth = 100;
	private int    currentHealth;
	private string animationTriggerName;

	private Animator		  anim;
	private CapsuleCollider2D col;

	private HealthBar healthBar;
	public  bool	  isDead = false;


	public void Start()
	{
		animationTriggerName = "die";
		anim				 = GetComponent<Animator>();
		col					 = GetComponent<CapsuleCollider2D>();
		healthBar			 = GameObject.Find("Player Health Bar").GetComponent<HealthBar>();

		SetMaxHealth();
	}

	public virtual void TakeDamage(int damage)
	{
		currentHealth -= damage;
		healthBar.SetHealth(currentHealth);

		if (currentHealth <= 0)
			Die();
	}

	public void Die()
	{
		col.enabled = false;
		isDead		= true;
		anim.SetTrigger(animationTriggerName);

		if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0))
			StartCoroutine(StopTimeScale());
	}

	private IEnumerator StopTimeScale()
	{
		yield return new WaitForSeconds(1f);
		Time.timeScale = 0;
	}

	private void SetMaxHealth()
	{
		currentHealth = maxHealth;
		healthBar.SetMaxHealth(maxHealth);
	}
}
