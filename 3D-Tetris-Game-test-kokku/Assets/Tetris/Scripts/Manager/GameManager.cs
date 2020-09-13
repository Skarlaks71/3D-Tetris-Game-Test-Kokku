using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public GameObject optionsPainel;
	public GameObject faderObj;
	public Image faderImg;
	private bool _gameOver = false;
	private int _highScore;

	[SerializeField]
	private float _fadeSpeed = .02f;

	private Color _fadeTransparency = new Color(0, 0, 0, .04f);
	private string _currentScene;
	private AsyncOperation _async;

	private bool _isReturning = false;

	#region GameOver and CurrentScene properts
	public bool GameOver
	{
		set { _gameOver = value; }
		get { return _gameOver; }
	}

	public int HighScore
	{
		get { return _highScore; }
	}

	public string CurrentSceneName
	{
		get{ return _currentScene; }
	}
	#endregion

	void Awake()
	{
		// Only 1 Game Manager can exist at a time
		if (instance == null)
		{
			DontDestroyOnLoad(gameObject);
			instance = GetComponent<GameManager>();
			SceneManager.sceneLoaded += OnLevelFinishedLoading;
		}
		else
		{
			Destroy(gameObject);
		}
		if (PlayerPrefs.GetInt("HighScore") != 0 || PlayerPrefs.GetInt("HighScore") > 0)
		{
			_highScore = PlayerPrefs.GetInt("HighScore");
		}
		else
		{
			_highScore = 0;
		}
	}

	// Iterate the fader transparency to 0%
	IEnumerator FadeIn(GameObject faderObj, Image fader)
	{
		while (fader.color.a > 0)
		{
			fader.color -= _fadeTransparency;
			yield return new WaitForSeconds(_fadeSpeed);
		}
		faderObj.SetActive(false);
	}

	//Iterate the fader transparency to 100%
	IEnumerator FadeOut(GameObject faderObj, Image fader)
	{
		faderObj.SetActive(true);
		while(fader.color.a < 1)
		{
			fader.color += _fadeTransparency;
			yield return new WaitForSeconds(_fadeSpeed);
		}
		ActivateScene();//Activate scene when the fades end
	}

	// Begin loading a scene with a specified string asynchronously
	IEnumerator Load(string sceneName)
	{
		_async = SceneManager.LoadSceneAsync(sceneName);
		_async.allowSceneActivation = false;
		yield return _async;
		_isReturning = false;
	
	}

	void Update()
	{

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			ReturnToMenu();
		}

		if (Input.GetKeyDown(KeyCode.L))
		{
			PlayerPrefs.SetInt("HighScore", 0);
		}

	}

	private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		_currentScene = scene.name;
		instance.StartCoroutine(FadeIn(instance.faderObj, instance.faderImg));
	}

	public void ReturnToMenu()
	{
		if (_isReturning)
		{
			return;
		}

		if (CurrentSceneName != "MainMenu")
		{
			
			StopAllCoroutines();
			LoadScene("MainMenu");
			_isReturning = true;
			
		}
	}

	public void LoadScene(string sceneName)
	{
		instance.StartCoroutine(Load(sceneName));
		instance.StartCoroutine(FadeOut(instance.faderObj, instance.faderImg));
	}

	// Allows the scene to change once it is loaded
	public void ActivateScene()
	{
		_async.allowSceneActivation = true;
	}

	// Reload the current scene
	public void ReloadScene()
	{
		LoadScene(SceneManager.GetActiveScene().name);
	}

	public void ExitGame()
	{
		// If we are running in a standalone build of the game
		#if UNITY_STANDALONE
			// Quit the application
			Application.Quit();
		#endif

		// If we are running in the editor
		#if UNITY_EDITOR
			// Stop playing the scene
			UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}

	//ative painel settings of manager canvas
	public void OpenSettings(bool full)
	{
		if (full)
		{
			optionsPainel.SetActive(true);
			optionsPainel.transform.GetChild(5).gameObject.SetActive(true);
		}
		else
		{
			optionsPainel.SetActive(true);
		}
		
	}

	//return game time
	public void ReturnGame()
	{
		Time.timeScale = 1;
	}

	public void AddHighScore(int score)
	{
		_highScore = score;
		PlayerPrefs.SetInt("HighScore", _highScore);
	}
}
