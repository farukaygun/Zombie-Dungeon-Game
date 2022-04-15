using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private Transform target;

	private void Start()
	{
		target = GameObject.FindGameObjectWithTag("Player").transform;
	}

	private void FixedUpdate()
	{
		transform.position = Vector3.Lerp(new Vector3(transform.position.x, transform.position.y, -10f), 
			new Vector3(target.position.x, target.position.y, -10f), 
			Time.fixedDeltaTime * 10f);
	}
}
