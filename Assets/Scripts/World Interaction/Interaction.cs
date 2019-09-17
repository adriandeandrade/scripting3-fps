using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
	// Inspector Fields
	[Header("Interaction Configuration")]
	[SerializeField] private LayerMask interactableLayer;
	[SerializeField] private float maxInteractionDistance;

	// Private Variables
	private bool canInteract;
	private Interactable lastItemInteracted;

	// Componenets
	private Camera cam;

	private void InitializeInput()
	{
		Toolbox.instance.GetInputManager().interactionControls.performed += OnInteract;
	}

	private void Start()
	{
		InitializeInput();
	}

	private void Awake()
	{
		cam = Camera.main;
	}

	private void OnInteract(InputAction.CallbackContext context)
	{
		if (canInteract)
		{
			if (lastItemInteracted != null)
			{
				lastItemInteracted.Interact();
				//lastItemInteracted.DeactivateInteractionUI();
				lastItemInteracted = null;
			}
		}
	}

	private void Update()
	{
		GetInteraction();
	}

	private void GetInteraction()
	{
		if (lastItemInteracted != null)
		{
			//lastItemInteracted.DeactivateInteractionUI();
			lastItemInteracted = null;
		}

		RaycastHit hit;
		Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));

		if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, maxInteractionDistance, interactableLayer))
		{
			if (hit.collider != null)
			{
				lastItemInteracted = hit.collider.GetComponentInChildren<Interactable>();

				if (lastItemInteracted != null)
				{
					float distanceToObject = GetDistance(hit.collider.transform.position);

					if (distanceToObject <= maxInteractionDistance)
					{
						canInteract = true;
						//lastItemInteracted.ActivateInteractionUI();
					}
					else
					{
						canInteract = false;
						//lastItemInteracted.DeactivateInteractionUI();
						lastItemInteracted = null;
					}
				}
			}
		}
	}

	private float GetDistance(Vector3 objectPos)
	{
		return Vector3.Distance(transform.position, objectPos); ;
	}

}
