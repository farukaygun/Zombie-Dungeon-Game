using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
	Idle,
	Chase,
	Attack,
	Die
}

public class EnemyController : MonoBehaviour
{
	[Header("Attack")]
	[SerializeField] private Transform attackPoint;
	[SerializeField] private float 	   attackRange    = 0.5f;
	[SerializeField] private int	   damage 	      = 20;
	[SerializeField] private int	   attackCooldown = 2;
	[SerializeField] private float	   lastAttackTime;
	[SerializeField] private LayerMask playerLayers;


	private float 	  speed;
	private Transform target;
	private Animator  anim;
	public State      currentState;

	private void Start() 
	{
		anim   		 = GetComponent<Animator>();
		target 		 = GameObject.FindGameObjectWithTag("Player").transform;
		playerLayers = LayerMask.GetMask("Player");
		speed		 = 1f;
	}

	private void Update() 
	{
		StateCheck();
		StateExecute();
	}

	private void StateExecute()
	{
		switch (currentState)
		{
			case State.Idle:
				break;
			case State.Chase:
				FollowTarget();
				break;
			case State.Attack:
				Attack();
				break;
		}
	}

	private void StateCheck()
	{
		float distanceToTarget = Vector3.Distance(target.position, transform.position);

		if (currentState != State.Die)
		{
			if (distanceToTarget > attackRange)
				currentState = State.Chase;
			else currentState = State.Attack;
		}
	}

	private void FollowTarget() 
	{
		anim.SetBool("isRunning", true);
		transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime * 0.1f);

		// look at target
		if (transform.position.x > target.position.x && transform.localScale.x > 0)
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		else if (transform.position.x < target.position.x && transform.localScale.x < 0)
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}

	private void Attack()
	{
		if (Time.time > lastAttackTime + attackCooldown)
		{
			anim.SetBool("isRunning", false);
			anim.SetTrigger("attack");

			Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);

			foreach (var player in hitPlayer)
				player.GetComponent<PlayerHealth>().TakeDamage(damage);

			lastAttackTime = Time.time;
		}
	}
}
