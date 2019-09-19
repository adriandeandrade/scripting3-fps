using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	public Inventory inventory;
	public Player player;

	private void Awake()
	{
		player = FindObjectOfType<Player>();
		inventory = player.gameObject.AddComponent<Inventory>();
	}
}
