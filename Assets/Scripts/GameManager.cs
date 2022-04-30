using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public Transform rightSpawner;
	public Transform leftSpawner;
	private float spawnCountDown;
	private bool isSpawning;
	public bool isGameOver;

	[SerializeField] private GameObject panelGameOver;
	[SerializeField] private Button buttonRestart;
	[SerializeField] private Button buttonExit;
	[SerializeField] private TMP_Text textScore;

	[SerializeField] private EnemyPool enemyPool;
	[SerializeField] private Score score;


	private void Awake()
	{
		instance = this;
	}

	private void OnEnable()
	{
		buttonExit.onClick.AddListener(() => ExitGame());
		buttonRestart.onClick.AddListener(() => RestartGame());
	}

	private void Start() 
	{
		Time.timeScale = 1;
		isSpawning 	   = true;
		spawnCountDown = 5f;

		StartCoroutine(SpawnRoutine());
	}

	private void Update()
	{
		if (isGameOver)
			GameOver();
	}

	private IEnumerator SpawnRoutine()
	{
		while (isSpawning)
		{
			yield return new WaitForSeconds(spawnCountDown);

			GameObject enemy = enemyPool.GetPooledObject();
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

	private void GameOver()
	{
		panelGameOver.SetActive(true);
		textScore.text = "SCORE: " + score.GetScore().ToString();	
	}

	private void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	private void ExitGame()
	{
		Application.Quit();
	}
}
