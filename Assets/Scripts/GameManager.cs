using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	public static GameManager instance = null;

	// Add refernce to LevelManager

	private bool _doingSetup = true;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}

		InitGame();
	}

	void InitGame()
	{
		// Setup scene in level manager
	}
}
