using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
	[SerializeField] private Sprite originalCrosshair;
	[SerializeField] private Sprite interactionIcon;
	[SerializeField] private Image crosshairImage;
    [SerializeField] private GameObject interactionKeyUI;

	bool showingInteractionIcon;

	private void Awake()
	{
        originalCrosshair = Resources.Load<Sprite>("OriginalCrosshair");
        interactionIcon = Resources.Load<Sprite>("InteractionCrosshair");
        crosshairImage = GameObject.FindGameObjectWithTag("CrosshairUI").GetComponent<Image>();
        interactionKeyUI = crosshairImage.transform.parent.GetChild(1).gameObject;
	}

	private void Start()
	{
		crosshairImage.sprite = originalCrosshair;
		showingInteractionIcon = false;
        interactionKeyUI.SetActive(false);
	}

	public void ShowInteractionIcon()
	{
		if (!showingInteractionIcon)
		{
			showingInteractionIcon = true;
            interactionKeyUI.SetActive(true);
			crosshairImage.sprite = interactionIcon;
		}
	}

	public void ShowCrosshair()
	{
		if (showingInteractionIcon)
		{
			showingInteractionIcon = false;
            interactionKeyUI.SetActive(false);
			crosshairImage.sprite = originalCrosshair;
		}
	}
}
