using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
	// Inspector Fields
	[Header("Weapon Configuration")]
	[SerializeField] protected WeaponData weaponData;
	[SerializeField] protected LayerMask ignoreMask;
	[SerializeField] protected GameObject bulletHolePrefab;
	[SerializeField] protected bool debug;

	protected enum WeaponStates { READY, RELOADING, CYCLING, NOAMMO };
	protected WeaponStates weaponState = WeaponStates.READY;

	// Private Variables
	protected int bulletsLeftInMagazine;
	protected int bulletsLeftBeforeReload;

	protected float currentCycleTime;

	protected bool isCycling = false;

	// Components
	protected Camera cam;
	protected Player player;
	protected Animator animator;
	protected Inventory inventory;

	// Properties
	public bool IsCycling { get => isCycling; }
	public int BulletsLeftInMagazine { get => bulletsLeftInMagazine; }
	public WeaponData WeaponData { get => weaponData; }

	private void OnEnable()
	{
		Inventory.OnItemAdded += UpdateAmmoUI;
		Player.OnWeaponCycled += UpdateAmmoUI;
	}

	private void Awake()
	{
		cam = Camera.main;
		player = FindObjectOfType<Player>(); // Get in GameManager and use that as reference to player. [Toolbox.instance.GetGameManager.PlayerRef]
		animator = GetComponent<Animator>();
	}

	protected virtual void Start()
	{
		inventory = Toolbox.instance.GetInventoryManager().inventory;

		InitializeWeapon();
		Debug.Log("Weapon Started");
	}

	private void Update()
	{
		UpdateStates();
	}

	protected void InitializeWeapon()
	{
		bulletsLeftInMagazine = weaponData.magazineCapacity;
		SetState(WeaponStates.READY);
		UpdateAmmoUI();
	}

	private void LateUpdate()
	{
		if (CanReload())
		{
			StartReload();
		}
	}

	public void Shoot()
	{
		if (CanShoot())
		{
			ShootRay();
			OnStartCycle();
			bulletsLeftInMagazine--;
			UpdateAmmoUI();
			SetState(WeaponStates.CYCLING);
		}
	}

	public bool CanShoot()
	{
		if (!player.IsReloading && !isCycling && bulletsLeftInMagazine > 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool CanReload()
	{
		if (!player.IsReloading && bulletsLeftInMagazine == 0 && inventory.CheckIfItemExists(weaponData.ammoType))
		{
			return true;
		}

		return false;
	}

	protected abstract void ShootRay();

	public void StartReload()
	{
		if (!player.IsReloading && bulletsLeftInMagazine != weaponData.magazineCapacity)
		{
			bulletsLeftBeforeReload = bulletsLeftInMagazine;
			StartCoroutine(Reload());
		}
	}

	public void CancelReload()
	{
		player.IsReloading = false;
		bulletsLeftInMagazine = bulletsLeftBeforeReload;
	}

	private IEnumerator Reload()
	{
		player.IsReloading = true;

		yield return new WaitForSeconds(0.2f);

		float reloadSpeed = 1f / weaponData.reloadTime;
		float percent = 0;

		while (percent < 1)
		{
			percent += Time.deltaTime * reloadSpeed;
			yield return null;
		}

		player.IsReloading = false;

		int ammoAmountInInventory = inventory.CheckItemCount(weaponData.ammoType);
		if (ammoAmountInInventory > 0)
		{
			if (ammoAmountInInventory >= weaponData.magazineCapacity)
			{
				ammoAmountInInventory = weaponData.magazineCapacity;
			}
		}

		inventory.RemoveItem(weaponData.ammoType, ammoAmountInInventory);
		bulletsLeftInMagazine = ammoAmountInInventory;
		UpdateAmmoUI();
		SetState(WeaponStates.READY);
	}

	private void UpdateAmmoUI()
	{
		Toolbox.instance.GetUIManager().WeaponAmmoUI.UpdateWeaponAmmoUI(this);
	}

	public void SpawnBulletHole(List<RaycastHit> hitPoints)
	{
		foreach (RaycastHit hitPoint in hitPoints)
		{
			Vector3 spawnPos = new Vector3(hitPoint.point.x, hitPoint.point.y, hitPoint.point.z - 0.1f);
			GameObject bulletHole = Instantiate(bulletHolePrefab, spawnPos, Quaternion.LookRotation(hitPoint.normal));
			bulletHole.transform.parent = hitPoint.transform;
		}
	}

	public void SpawnBulletHole(RaycastHit hitPoint)
	{
		Vector3 spawnPos = new Vector3(hitPoint.point.x, hitPoint.point.y, hitPoint.point.z - 0.1f);
		Instantiate(bulletHolePrefab, spawnPos, Quaternion.LookRotation(hitPoint.normal));
	}


	//	OnStartCycle: This gets called eveytime the weapon cycles. (After every shot)
	public void OnStartCycle()
	{
		SetState(WeaponStates.CYCLING);
	}


	// OnCycleFinish: This gets called after the weapon is finished cycling using an animation event.
	public void OnCycleFinish()
	{
		isCycling = false;
		Debug.Log("On cycle finish");
	}

	#region State Handling

	private void SetState(WeaponStates newState)
	{
		switch (newState)
		{
			case WeaponStates.CYCLING:
				weaponState = WeaponStates.CYCLING;
				InitCyclingState();
				break;

			case WeaponStates.NOAMMO:
				weaponState = WeaponStates.NOAMMO;
				InitNoAmmoState();
				break;

			case WeaponStates.READY:
				weaponState = WeaponStates.READY;
				InitReadyState();
				break;

			case WeaponStates.RELOADING:
				weaponState = WeaponStates.RELOADING;
				InitReloadingState();
				break;
		}
	}

	private void UpdateStates()
	{
		switch (weaponState)
		{
			case WeaponStates.CYCLING:
				UpdateCyclingState();
				break;

			case WeaponStates.NOAMMO:
				UpdateNoAmmoState();
				break;

			case WeaponStates.READY:
				UpdateReadyState();
				break;

			case WeaponStates.RELOADING:
				UpdateReloadingState();
				break;
		}
	}

	private void InitCyclingState()
	{
		isCycling = true;
	}

	private void InitNoAmmoState()
	{

	}

	private void InitReadyState()
	{
		if (bulletsLeftInMagazine <= 0)
		{

			SetState(WeaponStates.RELOADING);
		}
	}

	private void InitReloadingState()
	{
		if (CanReload())
		{
			StartReload();
		}
		else
		{
			SetState(WeaponStates.NOAMMO);
		}
	}

	private void UpdateCyclingState()
	{
		if(!isCycling)
		{
			SetState(WeaponStates.READY);
		}

	}

	private void UpdateNoAmmoState()
	{

	}

	private void UpdateReadyState()
	{

	}

	private void UpdateReloadingState()
	{

	}

	#endregion
}
