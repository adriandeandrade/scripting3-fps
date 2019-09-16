using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : InteractableItem
{
	public override void Interact()
	{
		Debug.Log("Interacted.");
		base.Interact();
		Destroy(gameObject);
	}
}
