using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour
{
	ScoreManager score = null;
	[SerializeField]
	Text scoreText = null;

	private void Start()
	{
		score = GameObject.FindWithTag("GameController").GetComponentInChildren<ScoreManager>();
	}

	private void Update()
	{
		if (scoreText != null)
		{
			scoreText.text = "" + score.Score;
		}
	}
}
