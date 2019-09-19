﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Player : BaseDamageable
{
	// Inspector Fields
	[Header("Player Configuration")]
	[SerializeField] private List<Weapon> weapons = new List<Weapon>();
	[SerializeField] private Weapon currentWeapon;
	[SerializeField] private Weapon otherWeapon;

	// Private Variables
	private bool isReloading;
	private bool isHoldingWeapon;

	// Components
	private Camera cam;

	public bool IsReloading { get => isReloading; set => isReloading = value; }
	public bool IsHoldingWeapon { get => isHoldingWeapon; set => isHoldingWeapon = value; }
	public Weapon CurrentWeaponInHand { get => currentWeapon; }

	// Events
	public delegate void OnWeaponCycledAction();
	public static event OnWeaponCycledAction OnWeaponCycled;

	protected override void Awake()
	{
		base.Awake();
		cam = Camera.main;
	}

	protected override void Start()
	{
		base.Start();
		InitializeInput();

		currentWeapon = weapons[0];
		currentWeapon.gameObject.SetActive(true);

		otherWeapon = weapons[1];
		otherWeapon.gameObject.SetActive(false);
	}

	private void CycleWeapons(InputAction.CallbackContext context)
	{
		if (CanCycleWeapons())
		{
			Weapon oldWeapon = currentWeapon;
			oldWeapon.gameObject.SetActive(false);
			currentWeapon = otherWeapon;
			otherWeapon = oldWeapon;
			currentWeapon.gameObject.SetActive(true);

			if (OnWeaponCycled != null)
			{
				OnWeaponCycled.Invoke();
			}
		}
	}

	private bool CanCycleWeapons()
	{
		return weapons.Count > 1 && !currentWeapon.IsCycling;
	}

	private void InitializeInput()
	{
		Toolbox.instance.GetInputManager().weaponControls.performed += OnShoot;
		Toolbox.instance.GetInputManager().cycleWeaponControls.performed += CycleWeapons;
	}

	private void OnShoot(InputAction.CallbackContext context)
	{
		if (currentWeapon != null)
		{
			currentWeapon.Shoot();
		}
	}
}
