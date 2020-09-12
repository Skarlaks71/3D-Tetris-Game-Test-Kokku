using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	public static SpawnManager instance;

	public GameObject[] peaces;
	public GameObject spawnPoint;
	private int _peaceDrawn = 0;

	void Awake()
	{
		instance = GetComponent<SpawnManager>();
	}

	void Start()
	{
		_peaceDrawn = Random.Range(0, peaces.Length);
		SpawnCopy();
	}
	// Update is called once per frame
	void Update () {

	}
	public void SpawnPeacePreview()
	{
		_peaceDrawn = Random.Range(0, peaces.Length);
		GameObject previousPeace = (GameObject)Instantiate(peaces[_peaceDrawn],transform.position,Quaternion.identity);
		previousPeace.GetComponent<PeaceBehave>().enabled = false;
		previousPeace.transform.parent = GameObject.Find("PreviousT").transform;
	}

	public void SpawnCopy()
	{
		Instantiate(peaces[_peaceDrawn], spawnPoint.transform.position, Quaternion.identity);
		if(GameObject.Find("PreviousT").transform.childCount != 0)
		{
			Destroy(GameObject.Find("PreviousT").transform.GetChild(0).gameObject);
		}
		SpawnPeacePreview();
	}
}
