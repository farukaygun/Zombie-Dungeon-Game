using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
	Idle,
	Patrol,
	Chase,
	Attack,
	Die,
	Empty,
}

public class EnemyController : MonoBehaviour
{
	[Header("Patrol")]
	private List<Transform> moveSpots;
	private GameObject patrolPoints;
	private int randomSpot;

	[SerializeField] private float patrolSpeed;
	[SerializeField] private float waitTime;
	[SerializeField] private float startWaitTime;
	[SerializeField] private float speed;

	private Transform target;
	public  EnemyState currentState;

	[SerializeField] private Animator anim;
	[SerializeField] private EnemyAttack enemyAttack;
	[SerializeField] private EnemyHealth enemyHealth;

	private Score score;


	private void Start() 
	{
		target = GameObject.FindGameObjectWithTag("Player").transform;
		patrolPoints = GameObject.FindGameObjectWithTag("Patrol Points");
		moveSpots = new List<Transform>();

		foreach (Transform item in patrolPoints.transform)
			moveSpots.Add(item);

		randomSpot = Random.Range(0, moveSpots.Count);
		score = GameObject.Find("Game Manager").GetComponent<Score>();
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
				enemyAttack.Attack();
				break;
			case EnemyState.Die:
				enemyHealth.Die();
				score.IncreaseScore(10);
				currentState = EnemyState.Empty;
				break;
		}
	}

	private void StateCheck()
	{
		float distanceToTarget = Vector3.Distance(target.position, transform.position);

		if (currentState != EnemyState.Die && currentState != EnemyState.Empty)
		{
			if (distanceToTarget > 5f)
				currentState = EnemyState.Patrol;
			else if (distanceToTarget <= 5f && distanceToTarget > enemyAttack.GetAttackRange())
				currentState = EnemyState.Chase;
			else 
				currentState = EnemyState.Attack;
		}
	}

	private void ChaseTarget() 
	{
		anim.SetBool("isRunning", true);
		transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
		RotateToTarget(target.position);
	}

	private void Patrol()
	{
		anim.SetBool("isRunning", true);
		transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, patrolSpeed * Time.fixedDeltaTime);
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
}
