using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour, IInteractableUI
{	
    // Private Variables
    protected bool activeUI; 

    public abstract void ActivateInteractionUI();
	public abstract void DeactivateInteractionUI();
	public abstract void Interact();
}
