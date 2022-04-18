using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[Header("Movement")]
	[SerializeField] private InputAction movementInput;
	[SerializeField] private float       speed;

	private float horizontal;
	private float vertical;


	[Header("Attack")]
	[SerializeField] private InputAction attackInput;
	[SerializeField] private Transform   attackPoint;
	[SerializeField] private float       attackRange;
	[SerializeField] private int         damage;
	[SerializeField] private LayerMask   enemyLayers;


	private Rigidbody2D rb;
	private Animator anim;

	private void Start()
	{
		rb   		= GetComponent<Rigidbody2D>();
		anim 		= GetComponent<Animator>();
		enemyLayers = LayerMask.GetMask("Enemy");
		speed = 3.5f;
		attackRange = 0.5f;
		damage = 50;

		// Input Assign
		attackInput.performed   += _   => Attack();  // delegate method to InputAction
		movementInput.performed += cxt =>
		{
			horizontal = cxt.ReadValue<Vector2>().x;
			vertical   = cxt.ReadValue<Vector2>().y;
		};
		movementInput.canceled  += _   =>
		{
			horizontal = 0;
			vertical = 0;
		};
	}

	private void OnEnable()
	{
		movementInput.Enable();
		attackInput.Enable();
	}

	private void OnDisable()
	{
		movementInput.Disable();
		attackInput.Disable();
	}

	private void FixedUpdate()
	{
		Move();
	}

	private void Move()
	{
		Vector3 localHorizontalVector = transform.right * horizontal;
		Vector3 localVerticalVector   = transform.up * vertical;
		// We can move diagonally by adding two vectors.
		Vector3 movementVector        = localVerticalVector + localHorizontalVector;

		movementVector.Normalize(); // Normalized diagonal movement.

		movementVector *= speed * Time.fixedDeltaTime * 40f;
		rb.velocity     = movementVector;

		if (rb.velocity != Vector2.zero)
			SetRunAnimation(true);
		else
			SetRunAnimation(false);

	}

	private void SetRunAnimation(bool isRunning)
	{
		anim.SetBool("isRunning", isRunning);

		if (horizontal > 0)
			transform.localScale = new Vector3(0.33f, transform.localScale.y);
		else if (horizontal < 0)
			transform.localScale = new Vector3(-0.33f, transform.localScale.y);
	}

	private void Attack()
	{
		anim.SetTrigger("attack1");

		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
		
		foreach (var enemy in hitEnemies)
			enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
	}

	// display attack range with circle
	private void OnDrawGizmos() {
		if (attackPoint == null)
			return;

		Gizmos.DrawWireSphere(attackPoint.position, attackRange);
	}
}
