using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	public GameObject[] peaces;
	public GameObject spawnPoint;
	private bool _isGround = true;

	public bool IsGround{
		set { _isGround = value; }
	}

	// Update is called once per frame
	void Update () {
		if (_isGround)
		{
			_isGround = false;
			SpawnPeace();
		}
	}
	private void SpawnPeace()
	{
		Instantiate(peaces[Random.Range(0, peaces.Length)],transform.position,Quaternion.identity);
	}
}
