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

	private void Awake()
	{
		player = FindObjectOfType<Player>();
		inventory = player.gameObject.AddComponent<Inventory>();
	}

	private void UpdateInventoryManagerReferences(Scene scene, LoadSceneMode mode)
	{
		player = FindObjectOfType<Player>();

		if(player.GetComponent<Inventory>())
		{
			inventory = player.GetComponent<Inventory>();
		}

		//inventory = player.gameObject.AddComponent<Inventory>();
	}
}
