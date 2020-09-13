using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PeaceBehave : MonoBehaviour {

	private float _lastTime = 0;
	

	void Start()
	{
		if (!ValidMove())
		{
			Debug.Log("Game Over");
			Destroy(gameObject);
			GameManager.instance.GameOver = true;
		}
	}
	// Update is called once per frame
	void Update () {
		
			Fall();
			Movement();
			RotationObject();
		
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
				SpawnManager.instance.SpawnCopy();
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
			transform.position += new Vector3(1, 0, 0) ;
			if (!ValidMove())
			{
				transform.position += new Vector3(-1, 0, 0);
			}
		}
		//Move Down
		if (Input.GetKey(KeyCode.S) && transform.position.y >= 3)
		{
			transform.position += new Vector3(0, -1, 0);
			if (!ValidMove())
			{
				transform.position += new Vector3(0, 1, 0);
				AddOnBoard();
				DeleteAllMatch();
				enabled = false;
				transform.parent = null;
				SpawnManager.instance.SpawnCopy();
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

	private void AddOnBoard()//add peaces on board
	{
		foreach(Transform child in transform)
		{
			int roundedX = Mathf.RoundToInt(child.transform.position.x);
			int roundedY = Mathf.RoundToInt(child.transform.position.y);

			Board.grid[roundedX, roundedY] = child;
		}
	}

	private bool ValidMove()//check if move is valid
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
				GUIManager.instance.AddScore();
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
			GameManager.instance.gameObject.GetComponent<SoundManager>().effectAS.Play();
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
