using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	[Header("Input Manager Configuration")]
	[SerializeField] private InputActionAsset playerControls;

	// Input Actions
	public InputAction interactionControls;
	public InputAction movementControls;
	public InputAction weaponControls;
	public InputAction cycleWeaponControls;
	public InputAction jumpControl;
	public InputAction reloadControl;

	private void OnEnable()
	{
		interactionControls.Enable();
		movementControls.Enable();
		weaponControls.Enable();
		cycleWeaponControls.Enable();
		jumpControl.Enable();
		reloadControl.Enable();
	}

	private void OnDisable()
	{
        interactionControls.Disable();
		movementControls.Disable();
		weaponControls.Disable();
		cycleWeaponControls.Disable();
		jumpControl.Disable();
		reloadControl.Disable();
	}

	private void Awake()
	{
        playerControls = Resources.Load<InputActionAsset>("InputMaster");
		InitializeInput();
	}

	private void InitializeInput()
	{
		InputActionMap playerActionMap = playerControls.GetActionMap("Player");
		
        interactionControls = playerActionMap.GetAction("Interaction");
		movementControls = playerActionMap.GetAction("Movement");
		weaponControls = playerActionMap.GetAction("Weapon Controls");
		cycleWeaponControls = playerActionMap.GetAction("Inventory Controls");
		jumpControl = playerActionMap.GetAction("Jump");
		reloadControl = playerActionMap.GetAction("Reload");
	}
}
