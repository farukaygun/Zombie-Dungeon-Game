using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public Transform rightSpawner;
	public Transform leftSpawner;
	private float 	 spawnCountDown;
	private bool	 isSpawning;

	private void Start() 
	{
		isSpawning 	   = true;
		spawnCountDown = 2f;

		StartCoroutine(SpawnRoutine());
	}

	private IEnumerator SpawnRoutine()
	{
		while (isSpawning == true)
		{
			yield return new WaitForSeconds(spawnCountDown);

			GameObject enemy = EnemyPool.instance.GetPooledObject();
			if (enemy != null)
			{
				int selectedSpawner = Random.Range(0, 2);
				Transform spawner   = selectedSpawner == 0 ? leftSpawner : rightSpawner;
				enemy.transform.position = spawner.position;
				enemy.transform.rotation = spawner.rotation;
				enemy.SetActive(true);
			}
		}
	}
}
