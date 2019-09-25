using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Interactable
{
	[System.Serializable]
	public struct CrateSlot
	{
		public Transform spawnPoint;
		public bool hasItemInSlot;
	}

	// Inspector Field
	[SerializeField] private List<Item> possibleItemSpawns;
	[SerializeField] private CrateSlot[] crateSlots;

	// Private Variables
	private bool isOpen = false;
	private CrateSlot targetSlot;

	// Components
	private Animator animator;
	private Collider interactionCollider;

	private void Awake()
	{
		animator = GetComponentInChildren<Animator>();
		interactionCollider = GetComponent<Collider>();
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
		animator.SetTrigger("Open");
		isOpen = true;
		interactionCollider.enabled = false;
		SpawnItems();
	}

	private void SpawnItems()
	{
		for (int i = 0; i < crateSlots.Length; i++)
		{
			int randomItemToSpawnIndex = Random.Range(0, possibleItemSpawns.Count);
			Item itemSpawned = possibleItemSpawns[randomItemToSpawnIndex];

			GameObject itemObject = Instantiate(itemSpawned.itemPrefab, crateSlots[i].spawnPoint.position, Quaternion.identity);
			itemObject.transform.parent = transform;
		}
	}

	IEnumerator PickRandomCrateSlot()
	{
		while (true)
		{
			int randomCrateSlotIndex = Random.Range(0, crateSlots.Length);
			if (crateSlots[randomCrateSlotIndex].hasItemInSlot)
			{
				yield return null;
			}
			else
			{
				CrateSlot slot = crateSlots[randomCrateSlotIndex];
				slot.hasItemInSlot = true;

				int randomItemToSpawnIndex = Random.Range(0, possibleItemSpawns.Count);
				Item itemSpawned = possibleItemSpawns[randomItemToSpawnIndex];

				GameObject itemObject = Instantiate(itemSpawned.itemPrefab, slot.spawnPoint.position, Quaternion.identity);

				yield break;
			}
		}
	}

	private bool SlotIsEmpty(CrateSlot slotToCheck)
	{
		if (slotToCheck.hasItemInSlot)
		{
			return false;
		}
		else
		{
			return true;
		}
	}
}
