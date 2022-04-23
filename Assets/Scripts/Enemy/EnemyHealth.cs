using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
	[Header("Health System")]
	private int				  maxHealth = 100;
	private int				  currentHealth;
	private string			  animationTriggerName = "die";

	private Animator		  anim;
	private CapsuleCollider2D col;

	[Header("Health Bar")]
	private float     offsetY = 0.5f;
	private Canvas	  canvas;

	private HealthBar healthBar;


	public void Start()
	{
		healthBar		   = transform.Find("Canvas/Enemy Health Bar").GetComponent<HealthBar>(); 
		canvas			   = transform.Find("Canvas").GetComponent<Canvas>();
		canvas.worldCamera = Camera.main;
		anim			   = GetComponent<Animator>();
		col				   = GetComponent<CapsuleCollider2D>();

		SetMaxHealth();
	}

	private void FixedUpdate()
	{
		HealthBarFollowTransform();
	}

	public void TakeDamage(int damage)
	{
		currentHealth -= damage;
		healthBar.SetHealth(currentHealth);

		if (currentHealth <= 0)
			Die();
	}

	private void Die()
	{
		GetComponent<EnemyController>().currentState = EnemyState.Die;
		col.enabled = false;
		anim.SetTrigger(animationTriggerName);
		StartCoroutine(ClearDeadBody());
	}

	private IEnumerator ClearDeadBody() {
		yield return new WaitForSeconds(3f);

		GetComponent<EnemyController>().currentState = EnemyState.Idle;
		col.enabled = true;
		SetMaxHealth();
		gameObject.SetActive(false);
	}

	private void HealthBarFollowTransform()
	{
		healthBar.transform.position = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);
	}

	private void SetMaxHealth()
	{
		currentHealth = maxHealth;
		healthBar.SetMaxHealth(maxHealth);
	}
}
