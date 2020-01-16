using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	public static GameManager instance = null;

	private LevelManager _levelManager;
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

		_levelManager = GetComponent<LevelManager>();
	}

	void Start()
	{
		InitGame();
	}

	void InitGame()
	{
		_levelManager.SetupScene();
	}

	public void FinishSetup()
	{
		_doingSetup = false;
	}
}
