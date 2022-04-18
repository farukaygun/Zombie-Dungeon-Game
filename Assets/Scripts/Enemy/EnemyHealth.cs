using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthSystem
{
	private float coolDown = 3f;

	public override void Die()
	{
		GetComponent<EnemyController>().currentState = State.Die;
		base.Die();
		StartCoroutine(ClearDeadBody());
	}

	private IEnumerator ClearDeadBody() {
		yield return new WaitForSeconds(coolDown);
		
		col.enabled = true;
		currentHealth = maxHealth;
		GetComponent<EnemyController>().currentState = State.Idle;
		gameObject.SetActive(false);
	}
}
