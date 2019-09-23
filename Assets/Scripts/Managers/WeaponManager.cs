using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class WeaponManager : MonoBehaviour
{
	private Transform hands;

	private List<Weapon> weapons = new List<Weapon>();
	private Weapon currentWeapon;
	private Weapon otherWeapon;

	public Weapon CurrentWeapon { get => currentWeapon; }

	public delegate void OnWeaponCycledAction();
	public static event OnWeaponCycledAction OnWeaponCycled;

	private void OnEnable()
	{
		SceneManager.sceneLoaded += UpdateWeaponManagerReferences;
	}

	private void Awake()
	{
		InitializeInput();

		//InitializeWeaponManager();
	}

	private void InitializeInput()
	{
		Toolbox.instance.GetInputManager().weaponControls.performed += OnShoot;
		Toolbox.instance.GetInputManager().reloadControl.performed += OnReload;
		Toolbox.instance.GetInputManager().cycleWeaponControls.performed += CycleWeapons;
	}

	private void InitializeWeaponManager()
	{
		hands = GameObject.FindGameObjectWithTag("Hands").transform;

		GetWeapons();

		currentWeapon = weapons[0];
		currentWeapon.gameObject.SetActive(true);

		otherWeapon = weapons[1];
		otherWeapon.gameObject.SetActive(false);
	}

	private void GetWeapons()
	{
		weapons.Clear();

		if (hands != null)
		{
			foreach (Transform weapon in hands)
			{
				Weapon _weapon = weapon.GetComponent<Weapon>();

				if (_weapon != null)
				{
					weapons.Add(_weapon);
				}
			}
		}
	}

	public void CycleWeapons(InputAction.CallbackContext context)
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

	private void OnShoot(InputAction.CallbackContext context)
	{
		if (currentWeapon != null)
		{
			currentWeapon.Shoot();
		}
	}

	private void OnReload(InputAction.CallbackContext context)
	{
		if (currentWeapon != null)
		{
			currentWeapon.StartReload();
		}
	}

	private void UpdateWeaponManagerReferences(Scene scene, LoadSceneMode mode)
	{
		InitializeWeaponManager();
	}
}
