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

	protected override void Awake()
	{
		base.Awake();
		cam = Camera.main;
	}

	protected override void Start()
	{
		base.Start();
		
		weaponManager = Toolbox.instance.GetWeaponManager();
		UpdateHealth();
	}

	private void UpdateHealth()
	{
		Debug.Log("Took damage");

		if (healthBar != null)
		{
			healthBar.fillAmount = currentHealth / startHealth;
		}
	}
}
