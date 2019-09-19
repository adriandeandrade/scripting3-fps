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
	private CrosshairController crosshairController;

	private void Awake()
	{
		InitializeSingelon();

		inventoryManager = gameObject.AddComponent<InventoryManager>();
		inputManager = gameObject.AddComponent<InputManager>();
		crosshairController = gameObject.AddComponent<CrosshairController>();
		uiManager = gameObject.AddComponent<UIManager>();
	}

	public InventoryManager GetInventoryManager()
	{
		return inventoryManager;
	}

	public InputManager GetInputManager()
	{
		return inputManager;
	}

	public CrosshairController GetCrosshairController()
	{
		return crosshairController;
	}

	public UIManager GetUIManager()
	{
		return uiManager;
	}
}
