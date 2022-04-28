using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private InputAction movementInput;
	[SerializeField] private float speed;

	private float horizontal;
	private float vertical;

	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private Animator anim;

	[SerializeField] private PlayerHealth playerHealth;


	private void Start()
	{
		// Input Assign
		movementInput.performed += cxt =>
		{
			horizontal = cxt.ReadValue<Vector2>().x;
			vertical   = cxt.ReadValue<Vector2>().y;
		};
		movementInput.canceled  += _   =>
		{
			horizontal = 0;
			vertical   = 0;
		};
	}

	private void OnEnable()
	{
		movementInput.Enable();
	}

	private void OnDisable()
	{
		movementInput.Disable();
	}

	private void FixedUpdate()
	{
		// if player died then don't move.
		if (playerHealth.isDead)
			return;

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
}
