using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIManager : MonoBehaviour {

	public static GUIManager instance;

	public GameObject scoreTxt;
	private int _score = 0;


	// Use this for initialization
	void Awake () {
		instance = GetComponent<GUIManager>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddScore()
	{
		_score += 10;
		scoreTxt.GetComponent<TextMeshProUGUI>().text = _score.ToString();
	}

	public void Return()
	{
		
	}
}
