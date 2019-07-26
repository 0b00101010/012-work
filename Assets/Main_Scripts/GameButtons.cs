using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameButtons : MonoBehaviour
{
	public GameObject gameUI;
	public GameObject gameoverUI;
	public GameObject pauseUI;

	[SerializeField]
	private Animator rockAnim;
	[SerializeField]
	private Animator illAnim;

	[SerializeField]
	private Text highScoreText;
	[SerializeField]
	private GameObject newScore;

	private GameManager gM;

	private bool isPause = false;


	private void Start()
	{
		gM = GetComponent<GameManager>();
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPauseButton();
        }
    }

    public void GameOverUI()
	{
		Time.timeScale = 1f;
		highScoreText.text = $": {gM.scoreRe.ToString()}점";

		gameUI.SetActive(false);
		gameoverUI.SetActive(true);
		pauseUI.SetActive(false);
	}

	public void RetryButton()
	{
        Time.timeScale = 1F;
		SceneManager.UnloadSceneAsync("Game");
		SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
	}

	public void newScoreAnim()
	{
		newScore.SetActive(true);
	}

	public void MainbackButton()
	{
        Time.timeScale = 1F;
		SceneManager.LoadScene("Main");
    }

	public void OnPauseButton()
	{
		if(isPause)
		{
			Time.timeScale = 0;
			isPause = false;
			pauseUI.SetActive(true);
			gameUI.SetActive(false);
		}
		else
		{
			Time.timeScale = 1;
			isPause = true;
			pauseUI.SetActive(false);
			gameUI.SetActive(true);
		}
	}
}
