using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
	[SerializeField]
	string startScene = "StartScreen";
	Coroutine routine = null;

	public void GameOver()
	{
		if (routine == null)
		{
			routine = StartCoroutine(GameOverRoutine());
		}
	}

	IEnumerator GameOverRoutine()
	{
		yield return null;
		SceneManager.LoadSceneAsync(startScene);
	}
}
