using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
	[SerializeField] private int damage;
	[SerializeField] private float attackRange;
	[SerializeField] private float lastAttackTime;
	[SerializeField] private float attackCooldown;

	[SerializeField] private InputAction attackInput;
	[SerializeField] private Transform attackPoint;
	[SerializeField] private LayerMask enemyLayers;
	[SerializeField] private Animator anim;

	[SerializeField] private PlayerHealth playerHealth;

	private void Start()
	{
		// Input Assign
		attackInput.performed += _ => Attack();  // delegate method to InputAction
	}

	private void OnEnable()
	{
		attackInput.Enable();
	}

	private void OnDisable()
	{
		attackInput.Disable();
	}

	private void Attack()
	{
		if (playerHealth.isDead)
			return;

		if (Time.time > lastAttackTime)
		{
			anim.SetTrigger("attack1");

			Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

			foreach (var enemy in hitEnemies)
				enemy.GetComponent<EnemyHealth>().TakeDamage(damage);

			lastAttackTime = Time.time + attackCooldown;
		}
	}

	// display attack range with circle
	private void OnDrawGizmos()
	{
		if (attackPoint == null)
			return;

		Gizmos.DrawWireSphere(attackPoint.position, attackRange);
	}
}
