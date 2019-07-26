using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
	public GameObject gameUI;
	public GameObject gameoverUI;

	[SerializeField]
	private Animator rockAnim;
	[SerializeField]
	private Animator illAnim;

	[SerializeField]
	private Text highScoreText;

	private GameManager gM;


	private void Start()
	{
		gM = GetComponent<GameManager>();
	}

	public void GameOverUI()
	{
		Time.timeScale = 1f;
		highScoreText.text = ": " + gM.scoreRe.ToString() + "점";
		gameUI.SetActive(false);
		gameoverUI.SetActive(true);
	}

	public void RetryButton()
	{
		SceneManager.UnloadSceneAsync("Game");
		SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
	}

	public void MainbackButton()
	{
		SceneManager.LoadScene("Main");
	}
}
