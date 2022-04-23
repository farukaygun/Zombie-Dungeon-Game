using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
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

	private float 	   speed;
	private Transform  target;
	private Animator   anim;
	public  EnemyState currentState;

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
			case EnemyState.Idle:
				break;
			case EnemyState.Patrol:
				Patrol();
				break;
			case EnemyState.Chase:
				ChaseTarget();
				break;
			case EnemyState.Attack:
				Attack();
				break;
		}
	}

	private void StateCheck()
	{
		float distanceToTarget = Vector3.Distance(target.position, transform.position);

		if (currentState != EnemyState.Die)
		{
			if (distanceToTarget > 5f)
				currentState = EnemyState.Patrol;
			else if (distanceToTarget <= 5f && distanceToTarget > attackRange)
				currentState = EnemyState.Chase;
			else 
				currentState = EnemyState.Attack;
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
			StartCoroutine(AttackRoutine());
			lastAttackTime = Time.time;
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
			{
				waitTime -= Time.deltaTime;
				anim.SetBool("isRunning", false);
			}
				
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

	// display attack range with circle
	private void OnDrawGizmos()
	{
		if (attackPoint == null)
			return;

		Gizmos.DrawWireSphere(attackPoint.position, attackRange);
	}
}
