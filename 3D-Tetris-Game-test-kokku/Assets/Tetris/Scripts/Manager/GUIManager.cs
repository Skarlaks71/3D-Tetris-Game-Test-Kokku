using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

	public static GUIManager instance;

	public Button optionBtn;
	public GameObject gameOverPainel;
	public GameObject scoreTxt;
	public GameObject highScoreTxt;
	public GameObject scoreOverTxt;
	private int _score = 0;


	// Use this for initialization
	void Awake () {
		instance = GetComponent<GUIManager>();
		if(SceneManager.GetActiveScene().name == "MainMenu")
		{
			scoreTxt = null;
			optionBtn = null;
			gameOverPainel = null;
			scoreOverTxt = null;
		}
		else
		{
			highScoreTxt.GetComponent<TextMeshProUGUI>().text = GameManager.instance.HighScore.ToString();
		}
		
	}
	
	
	// Update is called once per frame
	void Update () {
		if(GameManager.instance.GameOver == true){

			GameManager.instance.GameOver = false;
			GameOver();
			
		}
	}

	public void AddScore()
	{
		_score += 10;
		scoreTxt.GetComponent<TextMeshProUGUI>().text = _score.ToString();
	}

	private void ShowScore()
	{
		scoreOverTxt.GetComponent<TextMeshProUGUI>().text = _score.ToString();
	}

	public void OpenSettings(bool full)
	{
		GameManager.instance.OpenSettings(full);
	}
	public void PauseGame()
	{
		Time.timeScale = 0;
	}

	public void CallReturnToMenu()
	{
		GameManager.instance.ReturnToMenu();
	}
	private void GameOver()
	{

		optionBtn.enabled = false;
		ShowScore();
		gameOverPainel.SetActive(true);
		if (GameManager.instance.HighScore <= _score)
		{
			GameManager.instance.AddHighScore(_score);
		}
	}


}
