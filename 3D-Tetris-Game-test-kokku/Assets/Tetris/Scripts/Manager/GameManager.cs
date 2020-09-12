using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public AudioSource effectSound;
	public GameObject optionsPainel;
	public GameObject faderObj;
	public Image faderImg;
	private bool _gameOver = false;

	[SerializeField]
	private float _fadeSpeed = .02f;

	private Color _fadeTransparency = new Color(0, 0, 0, .04f);
	private string _currentScene;
	private AsyncOperation _async;

	private bool _isReturning = false;
	private bool _show = false;

	#region GameOver and CurrentScene properts
	public bool GameOver
	{
		set { _gameOver = value; }
		get { return _gameOver; }
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

		if (_show == true)
		{
			Debug.Log("up");
			
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

	public void OpenSettings(bool full)
	{
		if (full)
		{
			optionsPainel.SetActive(true);
			optionsPainel.transform.GetChild(4).gameObject.SetActive(true);
		}
		else
		{
			optionsPainel.SetActive(true);
		}
		
	}

	public void ReturnGame()
	{
		Time.timeScale = 1;
	}
}
