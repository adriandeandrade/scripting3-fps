using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Player : BaseDamageable
{
	// Private Variables
	private bool isReloading;
	private bool isHoldingWeapon;

	[SerializeField] private Weapon currentWeapon;

	private Camera cam;

	public bool IsReloading { get => isReloading; set => isReloading = value; }
	public bool IsHoldingWeapon { get => isHoldingWeapon; set => isHoldingWeapon = value; }

	private void Awake()
	{
		cam = Camera.main;
	}

	protected override void Start()
	{
		base.Start();
		InitializeInput();
	}

	private void InitializeInput()
	{
		Toolbox.instance.GetInputManager().weaponControls.performed += OnShoot;
	}

	private void OnShoot(InputAction.CallbackContext context)
	{	
		cam.transform.DOComplete();
		cam.transform.DOShakePosition(.1f, .1f, 10, 90, false, true);

		currentWeapon.Shoot();
		Debug.Log("Shot");
	}
}
