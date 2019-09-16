using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	// Inspector Fields
	[Header("Input Asset")]
	[SerializeField] private InputActionAsset playerControls;

	// Private Variables
	private bool isReloading;
	private bool isHoldingWeapon;

    [SerializeField] private Weapon currentWeapon;

	public bool IsReloading { get => isReloading; set => isReloading = value; }
	public bool IsHoldingWeapon { get => isHoldingWeapon; set => isHoldingWeapon = value; }

	// Input Actions
	private InputAction weaponControls;

	private void OnEnable()
	{
        weaponControls.Enable();
	}

	private void Awake()
	{
		InitializeInput();
	}

	private void InitializeInput()
	{
		InputActionMap playerActionMap = playerControls.GetActionMap("Player");
		weaponControls = playerActionMap.GetAction("Weapon Controls");

		weaponControls.performed += OnShoot;
		weaponControls.canceled += OnShoot;
	}

	private void OnShoot(InputAction.CallbackContext context)
	{
        currentWeapon.Shoot();
        Debug.Log("Shot");
	}
}
