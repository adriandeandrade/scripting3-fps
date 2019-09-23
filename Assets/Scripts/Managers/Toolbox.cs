using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class Toolbox : MonoBehaviour
{
	#region Singleton
	public static Toolbox instance;

	private void InitializeSingleton()
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
	private GameManager gameManager;
	private WeaponManager weaponManager;

	private void Awake()
	{
		InitializeSingleton();

		inventoryManager = gameObject.AddComponent<InventoryManager>();
		inputManager = gameObject.AddComponent<InputManager>();
		gameManager = gameObject.AddComponent<GameManager>();
		weaponManager = gameObject.AddComponent<WeaponManager>();
	}

	public InventoryManager GetInventoryManager()
	{
		return inventoryManager;
	}

	public InputManager GetInputManager()
	{
		return inputManager;
	}

	public GameManager GetGameManager()
	{
		return gameManager;
	}

	public WeaponManager GetWeaponManager()
	{
		return weaponManager;
	}
}
