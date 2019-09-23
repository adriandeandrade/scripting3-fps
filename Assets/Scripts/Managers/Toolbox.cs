using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toolbox : MonoBehaviour
{
	#region Singleton
	public static Toolbox instance;

	private void InitializeSingelon()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(this);
			return;
		}

		DontDestroyOnLoad(this);
	}

	#endregion

	private InventoryManager inventoryManager;
	private InputManager inputManager;
	private UIManager uiManager;
	private GameManager gameManager;

	private void Awake()
	{
		InitializeSingelon();

		inventoryManager = gameObject.AddComponent<InventoryManager>();
		inputManager = gameObject.AddComponent<InputManager>();
		uiManager = gameObject.AddComponent<UIManager>();
		gameManager = gameObject.AddComponent<GameManager>();
	}

	public InventoryManager GetInventoryManager()
	{
		return inventoryManager;
	}

	public InputManager GetInputManager()
	{
		return inputManager;
	}

	public UIManager GetUIManager()
	{
		return uiManager;
	}

	public GameManager GetGameManager()
	{
		return gameManager;
	}
}
