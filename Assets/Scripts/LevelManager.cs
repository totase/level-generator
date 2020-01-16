using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

	// Could be one room with room types as enums
	// For more information on room types, visit the read me
	[Header("Room objects")]
	public GameObject corridorRoom;
	public GameObject bottomRoom;
	public GameObject topRoom;
	public GameObject uniformRoom;

	// Should be moved to a room manager script
	[Header("Room properties")]
	[SerializeField] private int roomHeight = 6;
	[SerializeField] private int roomWidth = 8;

	[Header("Board properties")]
	[SerializeField] private int rows = 3;
	[SerializeField] private int columns = 3;

	private int _startColumn = 0;
	private bool _hasPath = false;

	private Transform _levelHolder;
	private List<Vector3> _gridPositions = new List<Vector3>();

	void InitialiseGrid()
	{
		_gridPositions.Clear();

		// Create list of positions for rooms
		for (int x = 0; x < columns; x += roomWidth)
		{
			for (int y = 0; y < rows; y += roomHeight)
			{
				_gridPositions.Add(new Vector3(x, y, 0f));
			}
		}
	}

	void RemoveFromPositions(Vector3 position)
	{
		_gridPositions.Remove(position);
	}

	// Function to get the position for a new room based on current direction
	Vector3 GetRoomPosition(int direction, Vector3 currentPos)
	{
		Vector3 _targetPos = Vector3.zero;

		switch (direction)
		{
		case 1: // Left
			if (currentPos.x == 0)
			{
				// If current position is 0 (on level edge), move down instead
				_targetPos.Set(currentPos.x, currentPos.y - roomHeight, 0f);
			}
			else
			{
				_targetPos.Set(currentPos.x - roomWidth, currentPos.y, 0f);
			}

			break;
		case 2: // Right
			if (currentPos.x == columns - roomWidth)
			{
				// If current position is the same as column value (on level edge), move down instead
				_targetPos.Set(currentPos.x, currentPos.y - roomHeight, 0f);
			}
			else
			{
				_targetPos.Set(currentPos.x + roomWidth, currentPos.y, 0f);
			}

			break;
		case 3: // Down
			_targetPos.Set(currentPos.x, currentPos.y - roomHeight, 0f);
			break;
		}

		return _targetPos;
	}

	// Function to get a new random direction
	int GetRandomDirection()
	{
		int _targetDirection = Random.Range(1, 6);

		switch (_targetDirection)
		{
		case 1:
		case 2:
			return 1; // Left
		case 3:
		case 4:
			return 2; // Right
		case 5:
			return 3; // Down
		default:
			Debug.LogError("Direction not found: " + _targetDirection);
			return 0;
		}
	}

	// Function to get a room type to instantiate, based on last, current, and target (next) position
	GameObject GetRoomType(Vector3 lastPos, Vector3 currentPos, Vector3 targetPos)
	{
		if (targetPos.y == currentPos.y)
		{
			if (lastPos.y > currentPos.y)
			{
				return bottomRoom;
			}

			return corridorRoom;
		}

		if (targetPos.y < currentPos.y)
		{
			if (currentPos.y == lastPos.y)
			{
				return topRoom;
			}

			if (targetPos.x < currentPos.x)
			{
				return corridorRoom;
			}
		}

		return uniformRoom;
	}

	void LevelSetup()
	{
		bool _hasEntrance = false;

		int _currentDir = 0;
		// Position to start room instantiation 
		float _startRow = rows - roomHeight;

		Vector3 _lastPos = new Vector3();
		Vector3 _targetPos = new Vector3();
		Vector3 _currentPos = new Vector3();

		while (!_hasPath)
		{
			if (!_hasEntrance)
			{
				_currentPos.Set(_startColumn, _startRow, 0f);

				_currentDir = 1;
				// If on an edge
				if (_currentPos.x == 0 && _currentDir == 1)
				{
					// Move the opposite direction
					_currentDir = 2;
				}
			}
			else
			{
				if (GetRandomDirection() == 3)
				{
					_currentDir = 3;
				}
			}

			_targetPos = GetRoomPosition(_currentDir, _currentPos);

			// If on a new level	
			if (_targetPos.y < _currentPos.y)
			{
				// Get new direction
				_currentDir = GetRandomDirection();

				// If on an edge
				if (_currentPos.x == 0 && _currentDir == 1)
				{
					// Move the opposite direction
					_currentDir = 2;
				}
				else if (_currentPos.x == columns - roomWidth && _currentDir == 2)
				{
					_currentDir = 1;
				}
			}

			if (_currentPos.y < 0)
			{
				_hasPath = true;

				break;
			}

			GameObject _toInstantiate;
			if (!_hasEntrance)
			{
				_hasEntrance = true;
				// Should be its own entrance room
				_toInstantiate = bottomRoom;
			}
			else
			{
				_toInstantiate = GetRoomType(_lastPos, _currentPos, _targetPos);
			}

			GenerateRoom(_toInstantiate, _currentPos);

			_lastPos = _currentPos;
			_currentPos = _targetPos;
		}

		FinishSetup();
	}

	// Function to instantiate a room on a given position
	// -- will also set the level object as a parent
	void GenerateRoom(GameObject room, Vector3 pos)
	{
		GameObject _currentRoom;
		RemoveFromPositions(pos);

		_currentRoom = Instantiate(room, pos, Quaternion.identity);
		_currentRoom.transform.SetParent(_levelHolder);
	}

	public void SetupScene()
	{
		// Get a random column to start on
		_startColumn = Random.Range(0, columns) * roomWidth;

		// Create a new level holder to keep hierarchy organized
		_levelHolder = new GameObject("Level").transform;

		// Multiply room width by columns to get correct values for x 
		columns = roomWidth * columns;
		// Do the same with the room height for y values
		rows = roomHeight * rows;

		// Set up grid with new column and row values --
		InitialiseGrid();
		// -- and set up the level
		LevelSetup();
	}

	void FinishSetup()
	{
		// Add side rooms

		GameManager.instance.FinishSetup();
	}
}
