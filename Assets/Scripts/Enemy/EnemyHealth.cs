using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthSystem
{
	private float  coolDown = 3f;
	private float  offsetY  = 0.5f;
	private Canvas canvas;

	public override void Start()
	{
		healthBar		   = transform.Find("Canvas/Enemy Health Bar").GetComponent<HealthBar>(); 
		canvas			   = transform.Find("Canvas").GetComponent<Canvas>();
		canvas.worldCamera = Camera.main;

		base.Start();
	}

	private void FixedUpdate()
	{
		HealthBarFollowTransform();
	}

	public override void Die()
	{
		GetComponent<EnemyController>().currentState = State.Die;
		base.Die();
		StartCoroutine(ClearDeadBody());
	}

	private IEnumerator ClearDeadBody() {
		yield return new WaitForSeconds(coolDown);
		
		col.enabled = true;
		SetMaxHealth();
		GetComponent<EnemyController>().currentState = State.Idle;
		gameObject.SetActive(false);
	}

	private void HealthBarFollowTransform()
	{
		healthBar.transform.position = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);
	}
}
