using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Inventory))]
public class InventoryInspector : Editor
{
	Item itemToAdd = null;
	int amountToAdd;

    public override void OnInspectorGUI()
	{
		/* DrawDefaultInspector();

		Inventory inventoryTarget = (Inventory)target;

		itemToAdd = EditorGUILayout.ObjectField(itemToAdd, typeof(Item), false) as Item;
		amountToAdd = EditorGUILayout.IntField(amountToAdd);

		if(GUILayout.Button("Add Item"))
		{
			inventoryTarget.AddItem(itemToAdd, amountToAdd);
		}

		if(GUILayout.Button("Print Inventory"))
		{
			inventoryTarget.PrintInventory();
		} */
	}
}
