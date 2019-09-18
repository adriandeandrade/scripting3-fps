using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	private Dictionary<Item, int> itemDict = new Dictionary<Item, int>();

	//private List<Item> items = new List<Item>();

	public void AddItem(Item itemToAdd, int amountToAdd)
	{
		if (CheckIfItemExists(itemToAdd))
		{
			itemDict[itemToAdd] += amountToAdd;
		}
		else
		{
			itemDict.Add(itemToAdd, amountToAdd);
		}

		Debug.Log("Added: " + amountToAdd + " of " + itemToAdd.itemName);
	}

	public void RemoveItem(Item itemToRemove, int amountToRemove)
	{
		if (CheckIfItemExists(itemToRemove))
		{
			itemDict[itemToRemove] -= amountToRemove;

			int amountLeft = GetCurrentAmount(itemToRemove);
			if (amountLeft <= 0)
			{
				itemDict.Remove(itemToRemove);
			}
		}

		Debug.Log("Removed: " + amountToRemove + " of " + itemToRemove.itemName);
	}

	public bool UseItem(Item itemToUse, int amountToUse, bool useAll = false)
	{
		if (CheckIfItemExists(itemToUse))
		{
			RemoveItem(itemToUse, amountToUse);
			return true;
		}
		else
		{
			return false;
		}
	}

	public int CheckItemCount(Item itemToCheck)
	{
		if (CheckIfItemExists(itemToCheck))
		{
			int amount = GetCurrentAmount(itemToCheck);
			return amount;
		}
		else
		{
			return 0;
		}
	}

	public bool CheckIfItemExists(Item itemToCheck)
	{
		if (itemDict.ContainsKey(itemToCheck))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public int GetCurrentAmount(Item itemToCheck)
	{
		int amountLeft = itemDict[itemToCheck];
		return amountLeft;
	}

	[ContextMenu("Print Inventory")]
	public void PrintInventory()
	{
		foreach (KeyValuePair<Item, int> item in itemDict)
		{
			Debug.Log(item.Key.itemName + ": " + item.Value);
		}
	}
}
