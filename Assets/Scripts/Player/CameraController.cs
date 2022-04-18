using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* ! No need because of started use Cinemachine 
*/

public class CameraController : MonoBehaviour
{
	[SerializeField] private Transform target;
	// [SerializeField] private Camera cam;
	// private float width, height;
	// private float minX, maxX;
	// private float minY, maxY;

	private void Start()
	{
		target = GameObject.FindGameObjectWithTag("Player").transform;
		// cam    = GetComponent<Camera>();
		// height = 2f * cam.orthographicSize;
		// width  = height * cam.aspect;
		// minX   = -15;
		// minY   = -11;
		// maxX   =  15;
		// maxY   =  11; 
	}

	private void FixedUpdate()
	{
		transform.position = Vector3.Lerp(new Vector3(transform.position.x, transform.position.y, -10f), 
			new Vector3(target.position.x, target.position.y, -10f), 
			Time.fixedDeltaTime * 5f);
	}
}
