using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScene : MonoBehaviour
{
    public string StartScene = "StartScreen";
    public Text ScoreText;
    
    // Start is called before the first frame update
    void Start()
    {
        var scoreManager = GameObject.FindWithTag("GameController").GetComponent<ScoreManager>();
        ScoreText.text = scoreManager.Score.ToString();
        Destroy(scoreManager.gameObject);
    }

    public void Event_GoToMainPage()
    {
        SceneManager.LoadSceneAsync(StartScene);
    }
}
