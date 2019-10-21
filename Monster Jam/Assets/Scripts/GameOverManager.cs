using System.Collections;
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
        DontDestroyOnLoad(gameObject);
		yield return null;
		SceneManager.LoadSceneAsync(startScene);
	}
}
