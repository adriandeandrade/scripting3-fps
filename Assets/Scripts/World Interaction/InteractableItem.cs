using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableItem : Interactable
{
	// Inspector Fields
	[Header("Interactable Item Data")]
	[SerializeField] protected Item item;

	// Components
	private TextMeshProUGUI itemNameText;

	private void Awake()
	{
        itemNameText = GetComponentInChildren<TextMeshProUGUI>();
	}

	private void Start()
	{
		itemNameText.SetText(item.itemName);
        DeactivateInteractionUI();
	}

	public override void Interact()
	{
		HandleItem(item);
		Destroy(gameObject);
	}

	private void HandleItem(Item _item)
	{
		Toolbox.instance.GetInventoryManager().inventory.AddItem(item, 1);
	}

	public override void ActivateInteractionUI()
	{
		if (!activeUI)
		{
			activeUI = true;
			itemNameText.enabled = true;
		}
	}

	public override void DeactivateInteractionUI()
	{
		if (activeUI)
		{
			activeUI = false;
			itemNameText.enabled = false;
		}
	}
}
