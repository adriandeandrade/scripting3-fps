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
	private CrosshairController crosshairController;

	// Componenets
	private Camera cam;

	private void InitializeInput()
	{
		Toolbox.instance.GetInputManager().interactionControls.performed += OnInteract;
	}

	private void Awake()
	{
		cam = Camera.main;
		crosshairController = FindObjectOfType<CrosshairController>();
	}

	private void Start()
	{
		InitializeInput();
	}

	private void OnInteract(InputAction.CallbackContext context)
	{
		if (canInteract)
		{
			if (lastItemInteracted != null)
			{
				lastItemInteracted.Interact();
				crosshairController.ShowCrosshair();
				lastItemInteracted = null;
				canInteract = false;
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
			crosshairController.ShowCrosshair();
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
						crosshairController.ShowInteractionIcon();
					}
					else
					{
						canInteract = false;
						crosshairController.ShowCrosshair();
						lastItemInteracted = null;
					}
				}
			}
		}
		else
		{
			crosshairController.ShowCrosshair();
		}
	}

	private float GetDistance(Vector3 objectPos)
	{
		return Vector3.Distance(transform.position, objectPos);
	}

}
