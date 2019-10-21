using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
	[SerializeField]
	string gameScene = null;

	[SerializeField]
	GameObject tutorialPanel = null;

	public void StartMatch()
	{
		SceneManager.LoadSceneAsync(gameScene);
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void ToggleTutorial()
	{
		if (tutorialPanel != null)
		{
			tutorialPanel.SetActive(!tutorialPanel.activeSelf);
		}
	}
}
