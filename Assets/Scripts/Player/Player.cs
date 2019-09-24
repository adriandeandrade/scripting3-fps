using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : BaseDamageable
{
	// Inspector Fields
	[Header("Player Configuration")]
	[SerializeField] private Image healthBar;
	[SerializeField] private Item healthPackItem;

	// Private Variables
	private bool isReloading;
	private bool isHoldingWeapon;

	// Components
	private Camera cam;
	private WeaponManager weaponManager;

	public bool IsReloading { get => isReloading; set => isReloading = value; }
	public bool IsHoldingWeapon { get => isHoldingWeapon; set => isHoldingWeapon = value; }

	public Weapon CurrentWeaponInHand
	{
		get
		{
			if(Toolbox.instance.GetWeaponManager())
			{
				return Toolbox.instance.GetWeaponManager().CurrentWeapon;
			}

			return null;
		}
	}

	private void OnEnable()
	{
		BaseDamageable.OnTakeDamageEvent += UpdateHealth;
	}

	protected void Awake()
	{
		cam = Camera.main;
	}

	protected override void Start()
	{
		InitializeInput();

		base.Start();
		currentHealth = 5f;
		weaponManager = Toolbox.instance.GetWeaponManager();
		UpdateHealth();
	}

		private void InitializeInput()
	{
		Toolbox.instance.GetInputManager().useHealthpackControl.performed += UseHealthPack;
	}

	private void UpdateHealth()
	{
		//Debug.Log("Took damage");

		if (healthBar != null)
		{
			healthBar.fillAmount = currentHealth / startHealth;
		}
	}

	public void AddHealth(float amountToAdd)
	{
		currentHealth += amountToAdd;
		currentHealth = Mathf.Clamp (currentHealth, currentHealth, startHealth);
		UpdateHealth();
	}

	public void UseHealthPack(InputAction.CallbackContext context)
	{
		if(Toolbox.instance.GetInventoryManager().inventory.UseItem(healthPackItem, 1))
		{
			AddHealth(5f);
		}
	}
}
