using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
	[SerializeField]
	string gameScene;

	public void StartMatch()
	{
		SceneManager.LoadSceneAsync(gameScene);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
