using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	// Private Variables
	private bool isReloading;
	private bool isHoldingWeapon;

    [SerializeField] private Weapon currentWeapon;

	public bool IsReloading { get => isReloading; set => isReloading = value; }
	public bool IsHoldingWeapon { get => isHoldingWeapon; set => isHoldingWeapon = value; }

	private void Start()
	{
		InitializeInput();
	}

	private void InitializeInput()
	{
		Toolbox.instance.GetInputManager().weaponControls.performed += OnShoot;
	}

	private void OnShoot(InputAction.CallbackContext context)
	{
        currentWeapon.Shoot();
        Debug.Log("Shot");
	}
}
