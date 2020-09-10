using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	public GameObject[] peaces;
	private bool _isGround = true;

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
		Instantiate(peaces[Random.Range(0, peaces.Length)],new Vector3(0,13,0),Quaternion.identity);
	}
}
