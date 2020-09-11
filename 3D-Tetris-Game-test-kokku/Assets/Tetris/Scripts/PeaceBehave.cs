using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PeaceBehave : MonoBehaviour {

	private bool _canMove = false;
	private float _lastTime = 0;

	public bool CanMove
	{
		set { _canMove = value; }
	}
	
	void Awake()
	{
		if (GameObject.Find("AtualT").transform.childCount > 0)
		{
			transform.parent = GameObject.Find("PreviousT").transform;
		}
		else
		{
			transform.parent = GameObject.Find("AtualT").transform;
			transform.position = GameObject.Find("SpawnPoint").transform.position;
			GameObject.Find("Spawner").GetComponent<SpawnManager>().IsGround = true;
			_canMove = true;
		}
	}
	// Update is called once per frame
	void Update () {
		if (_canMove)
		{
			transform.parent = GameObject.Find("AtualT").transform;
			Fall();
			Movement();
			RotationObject();
		}
		
	}

	private void Fall()
	{
		if (Time.time - _lastTime >= 1 && !Input.anyKeyDown)
		{
			transform.position += new Vector3(0, -1, 0);
			if (!ValidMove())
			{
				transform.position += new Vector3(0, 1, 0);
				AddOnBoard();
				DeleteAllMatch();
				enabled = false;
				transform.parent = null;
				GameObject.Find("PreviousT").transform.GetChild(0).transform.position = GameObject.Find("SpawnPoint").transform.position;
				GameObject.Find("PreviousT").transform.GetChild(0).GetComponent<PeaceBehave>().CanMove = true;
				GameObject.Find("Spawner").GetComponent<SpawnManager>().IsGround = true;
			}
			_lastTime = Time.time;
		}
		
	}
	private void Movement()
	{	
		//move left
		if (Input.GetKeyDown(KeyCode.A))
		{
			transform.position += new Vector3(-1, 0, 0);
			if (!ValidMove())
			{
				transform.position += new Vector3(1, 0, 0);
			}
		}
		//move right
		if (Input.GetKeyDown(KeyCode.D))
		{
			transform.position += new Vector3(1, 0, 0);
			if (!ValidMove())
			{
				transform.position += new Vector3(-1, 0, 0);
			}
		}
		//Move Down
		if (Input.GetKeyDown(KeyCode.S))
		{
			transform.position += new Vector3(0, -1, 0);
			if (!ValidMove())
			{
				transform.position += new Vector3(0, 1, 0);
				AddOnBoard();
				DeleteAllMatch();
				enabled = false;
				transform.parent = null;
				GameObject.Find("PreviousT").transform.GetChild(0).transform.position = GameObject.Find("SpawnPoint").transform.position;
				GameObject.Find("PreviousT").transform.GetChild(0).GetComponent<PeaceBehave>().CanMove = true;
				GameObject.Find("Spawner").GetComponent<SpawnManager>().IsGround = true;
			}
		}
	}
	private void RotationObject()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			transform.Rotate(new Vector3(0, 0, -90));
			if (!ValidMove())
			{
				transform.Rotate(new Vector3(0, 0, 90));
			}
		}
	}

	private void AddOnBoard()
	{
		foreach(Transform child in transform)
		{
			int roundedX = Mathf.RoundToInt(child.transform.position.x);
			int roundedY = Mathf.RoundToInt(child.transform.position.y);

			Board.grid[roundedX, roundedY] = child;
		}
	}

	private bool ValidMove()
	{
		foreach (Transform child in transform)
		{
			int roundedX = Mathf.RoundToInt(child.transform.position.x);
			int roundedY = Mathf.RoundToInt(child.transform.position.y);

			if(roundedX < 0 || roundedX >= Board.width || roundedY < 0 || roundedY >= Board.height+3)
			{
				return false;
			}
			if(Board.grid[roundedX,roundedY] != null)
			{
				return false;
			}
		}
		return true;
	}

	private bool DetectMatch(int posY)
	{
		for(int x=0; x<Board.width; x++)
		{
			if(Board.grid[x,posY] == null)
			{
				return false;
			}
		}

		return true;
	}

	private void DeleteAllMatch()
	{
		for(int y = 0; y < Board.height;y++) 
		{
			if (DetectMatch(y))
			{
				DeleteRow(y);
				DecreasePeaces(y);
				--y;
			}
		}
	}

	private void DeleteRow(int y)
	{
		for(int x = 0; x < Board.width; x++)
		{
			Destroy(Board.grid[x, y].gameObject);
			Board.grid[x, y] = null;
		}
		
	}
	private void DecreasePeaces(int posY)
	{
		for(int y = posY; y < Board.height; y++)
		{
			
			for(int x = 0; x < Board.width; x++)
			{
				if (Board.grid[x,y] != null)
				{
					Board.grid[x, y - 1] = Board.grid[x, y];
					Board.grid[x, y] = null;
					Board.grid[x, y-1].gameObject.transform.position += new Vector3(0, -1, 0);
				}
			}

		}
	}
}
