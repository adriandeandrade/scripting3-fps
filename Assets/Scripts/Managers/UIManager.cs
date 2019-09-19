using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	// Inspector Fields
	[SerializeField] private WeaponAmmoUI weaponAmmoUI;

    public WeaponAmmoUI WeaponAmmoUI { get => weaponAmmoUI; }

	private void Awake()
	{
        weaponAmmoUI = FindObjectOfType<WeaponAmmoUI>();
	}
}
