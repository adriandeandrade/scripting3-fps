using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Door : Interactable
{
	// Inspector Fields
	[Header("Door Configuration")]
	[SerializeField] private TextMeshProUGUI doorText;
	[SerializeField] private GameObject partToRemoveWhenUnlocked;
	[SerializeField] private Item keyNeededToUnlock;

	// Private Variables
	private bool unlocked = false;

	// Events
	public delegate void OnUnlockAction();
	public static event OnUnlockAction OnUnlock;

	public override void Interact()
	{
		Inventory inventory = Toolbox.instance.GetInventoryManager().inventory;

		if (inventory.CheckIfItemExists(keyNeededToUnlock))
		{
			inventory.UseItem(keyNeededToUnlock, 1);
            Unlock();
		}
        else
        {
            Debug.Log("Needed item not found.");
        }
	}

	private void Unlock()
	{
        partToRemoveWhenUnlocked.SetActive(false);
        unlocked = true;

		if(OnUnlock != null)
		{
			OnUnlock.Invoke();
		}
	}
}
