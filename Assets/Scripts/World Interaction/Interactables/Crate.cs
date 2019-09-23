using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Interactable
{
	// Inspector Field
	[SerializeField] private List<Item> possibleItemSpawns;
	[SerializeField] private List<Transform> possibleItemSpawnPoints;

	// Private Variables
	private bool isOpen = false;
	private Dictionary<Item, int> itemsInCrate = new Dictionary<Item, int>();

	// Components
	private Animator animator;
	private Collider interactionCollider;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		interactionCollider = GetComponent<Collider>();
		SpawnItems();
	}

	public override void Interact()
	{
		if (!isOpen)
		{
			OpenCrate();
		}
	}

	private void OpenCrate()
	{
		//SpawnItems();
		animator.SetTrigger("Open");
		isOpen = true;
		interactionCollider.enabled = false;

		foreach (KeyValuePair<Item, int> item in itemsInCrate)
		{
			Toolbox.instance.GetInventoryManager().inventory.AddItem(item.Key, item.Value);
		}

		itemsInCrate.Clear();

	}

	private void SpawnItems()
	{
		for (int i = 0; i < 3; i++)
		{
			int randomSpawnPointIndex = Random.Range(0, possibleItemSpawnPoints.Count);
			int randomItemToSpawnIndex = Random.Range(0, possibleItemSpawns.Count);
			Item itemSpawned = possibleItemSpawns[randomItemToSpawnIndex];

			if (itemsInCrate.ContainsKey(itemSpawned))
			{
				itemsInCrate[itemSpawned] += itemSpawned.minStackSize;
			}
			else
			{
				itemsInCrate.Add(itemSpawned, itemSpawned.minStackSize);
			}
		}
	}
}
