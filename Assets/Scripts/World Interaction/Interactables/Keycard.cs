using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keycard : InteractableItem
{
	// Inspector Fields
	[Header("Keycard Configuration")]
    [SerializeField] private Color cardColor;

	// Private Variables

	// Components
	private MeshRenderer mr;

	private void Awake()
	{
		mr = GetComponentInChildren<MeshRenderer>();
	}

	private void Start()
	{
       mr.materials[1].SetColor("_BaseColor", cardColor);
	}
}
