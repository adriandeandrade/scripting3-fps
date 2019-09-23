using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableItem : Interactable
{
	// Inspector Fields
	[Header("Interactable Item Data")]
	[SerializeField] protected Item item;
	[SerializeField] protected int stackSize;

	public int StackSize { get => stackSize; }

	public override void Interact()
	{
		HandleItem(item);
		Destroy(gameObject);
	}

	protected virtual void HandleItem(Item _item)
	{
		Toolbox.instance.GetInventoryManager().inventory.AddItem(item, stackSize);
	}
}
