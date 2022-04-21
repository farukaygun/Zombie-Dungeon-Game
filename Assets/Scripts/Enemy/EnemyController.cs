using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
	Idle,
	Patrol,
	Chase,
	Attack,
	Die
}

public class EnemyController : MonoBehaviour
{
	[Header("Attack")]
	[SerializeField] private Transform attackPoint;
	[SerializeField] private LayerMask playerLayers;
	[SerializeField] private float 	   attackRange    = 0.5f;
	[SerializeField] private int	   damage 	      = 20;
	[SerializeField] private int	   attackCooldown = 2;
	[SerializeField] private float	   lastAttackTime;

	[Header("Patrol")]
	private List<Transform> moveSpots = new List<Transform>();
	private GameObject patrolPoints;
	private int 	   randomSpot;
	private float 	   patrolSpeed;
	private float 	   waitTime;
	private float 	   startWaitTime;

	private float 	  speed;
	private Transform target;
	private Animator  anim;
	public  State     currentState;

	private void Start() 
	{
		anim   		  	  = GetComponent<Animator>();
		target 		  	  = GameObject.FindGameObjectWithTag("Player").transform;
		playerLayers  	  = LayerMask.GetMask("Player");
		speed		  	  = 2f;
		patrolPoints      = GameObject.FindGameObjectWithTag("Patrol Points");

		foreach (Transform item in patrolPoints.transform)
			moveSpots.Add(item);

		randomSpot		  = Random.Range(0, moveSpots.Count);
		patrolSpeed   	  = 1f;
		startWaitTime 	  = 7f;
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
			case State.Patrol:
				Patrol();
				break;
			case State.Chase:
				ChaseTarget();
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
			if (distanceToTarget > 5f)
				currentState = State.Patrol;
			else if (distanceToTarget <= 5f && distanceToTarget > attackRange)
				currentState = State.Chase;
			else 
				currentState = State.Attack;
		}
	}

	private void ChaseTarget() 
	{
		anim.SetBool("isRunning", true);
		transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime * 0.1f);
		RotateToTarget(target.position);
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

	private void Patrol()
	{
		anim.SetBool("isRunning", true);
		transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, patrolSpeed * Time.fixedDeltaTime * 0.1f);
		RotateToTarget(moveSpots[randomSpot].position);

		if (Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
		{
			if (waitTime <= 0)
			{
				randomSpot = GetNextPatrolPoint(3f);
				waitTime = startWaitTime;
			}
			else
				waitTime -= Time.deltaTime;
		}
	}

	private int GetNextPatrolPoint(float distanceToNextPoint)
	{
		int spot = randomSpot;
		while (Vector2.Distance(transform.position, moveSpots[spot].position) <= distanceToNextPoint)
		{
			spot = Random.Range(0, moveSpots.Count);
		}
		return spot;
	}

	private void RotateToTarget(Vector3 targetPosition)
	{
		if (transform.position.x > targetPosition.x && transform.localScale.x > 0)
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

		else if (transform.position.x < targetPosition.x && transform.localScale.x < 0)
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}
}
