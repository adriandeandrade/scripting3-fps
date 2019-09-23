using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
	// Inspector Fields
	[SerializeField] private WeaponAmmoUI weaponAmmoUI;

	public WeaponAmmoUI WeaponAmmoUI { get => weaponAmmoUI; }

	private void OnEnable()
	{
		SceneManager.sceneLoaded += UpdateUIManagerReferences;
	}

	private void Awake()
	{

		weaponAmmoUI = FindObjectOfType<WeaponAmmoUI>();
	}

	private void UpdateUIManagerReferences(Scene scene, LoadSceneMode mode)
	{
		weaponAmmoUI = FindObjectOfType<WeaponAmmoUI>();
	}

}
