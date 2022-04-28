using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
	[Header("Health System")]
	[SerializeField] private int maxHealth;
	[SerializeField] private int currentHealth;
	[SerializeField] private string animationTriggerName;

	[SerializeField] private Animator anim;
	[SerializeField] private CapsuleCollider2D col;

	[Header("Health Bar")]
	[SerializeField] private float offsetY;
	[SerializeField] private Canvas canvas;

	[SerializeField] private HealthBar healthBar;
	[SerializeField] private EnemyController enemyController;

	public void Start()
	{
		canvas.worldCamera = Camera.main;

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
			enemyController.currentState = EnemyState.Die;
	}

	public void Die()
	{
		col.enabled = false;
		anim.SetTrigger(animationTriggerName);
		StartCoroutine(ClearDeadBody());
	}

	private IEnumerator ClearDeadBody() {
		yield return new WaitForSeconds(3f);

		enemyController.currentState = EnemyState.Idle;

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
