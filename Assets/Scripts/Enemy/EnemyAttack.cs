using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
	[SerializeField] private int damage;
	[SerializeField] private int attackCooldown;
	[SerializeField] private float lastAttackTime;
	[SerializeField] private float attackRange;

	[SerializeField] private Transform attackPoint;
	[SerializeField] private LayerMask playerLayers;
	[SerializeField] private Animator anim;

	public void Attack()
	{
		if (Time.time > lastAttackTime)
		{
			StartCoroutine(AttackRoutine());
			lastAttackTime = Time.time + attackCooldown;
		}
	}

	private IEnumerator AttackRoutine()
	{
		anim.SetBool("isRunning", false);
		anim.SetTrigger("attack");

		yield return new WaitForSeconds(0.5f);

		Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);

		foreach (var player in hitPlayer)
			player.GetComponent<PlayerHealth>().TakeDamage(damage);
	}

	public float GetAttackRange()
	{
		return attackRange;
	}
}
