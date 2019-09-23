using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	public Inventory inventory;
	public Player player;

	private void OnEnable()
	{
		SceneManager.sceneLoaded += UpdateInventoryManagerReferences;
	}

	private void UpdateInventoryManagerReferences(Scene scene, LoadSceneMode mode)
	{
		if (player == null)
		{
			player = FindObjectOfType<Player>();
		}

		if (inventory == null)
		{
			inventory = new Inventory();
		}
	}
}
