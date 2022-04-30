using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
	[SerializeField] private int score;
	[SerializeField] private TMP_Text scoreText;

	private void Start()
	{
		scoreText.text += score.ToString();
	}

	public void IncreaseScore(int score)
	{
		this.score += score;
		UpdateScoreText();
	}

	public void UpdateScoreText()
	{
		scoreText.text = "SCORE: " + score.ToString();
	}

	public int GetScore()
	{
		return score;
	}
}
