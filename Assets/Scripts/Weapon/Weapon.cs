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

	protected enum WeaponStates { READY, RELOAD, CYCLING, NOAMMO };
	protected WeaponStates weaponState = WeaponStates.READY;

	// Private Variables
	protected int bulletsLeftInMagazine;
	protected int bulletsLeftBeforeReload;
	protected float nextFireTime;
	protected bool canShoot = true;

	// Components
	protected Camera cam;
	protected Player player;
	protected Animator animator;
	protected Inventory inventory;

	private void Awake()
	{
		cam = Camera.main;
		player = FindObjectOfType<Player>(); // Get in GameManager and use that as reference to player. [Toolbox.instance.GetGameManager.PlayerRef]
		animator = GetComponent<Animator>();
	}

	protected virtual void Start()
	{
		inventory = Toolbox.instance.GetInventoryManager().inventory;

		bulletsLeftInMagazine = weaponData.magazineCapacity;
		nextFireTime = Time.time + weaponData.fireRate;
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
			bulletsLeftInMagazine--;
			nextFireTime = Time.time + weaponData.fireRate;
			canShoot = false;
			//Debug.Log("Shot weapon");
		}
	}

	public void SpawnBulletHole(List<RaycastHit> hitPoints)
	{
		foreach (RaycastHit hitPoint in hitPoints)
		{
			Vector3 spawnPos = new Vector3(hitPoint.point.x, hitPoint.point.y, hitPoint.point.z - 0.1f);

			Instantiate(bulletHolePrefab, spawnPos, Quaternion.LookRotation(hitPoint.normal));
		}
	}

	public void SpawnBulletHole(RaycastHit hitPoint)
	{
			Vector3 spawnPos = new Vector3(hitPoint.point.x, hitPoint.point.y, hitPoint.point.z - 0.1f);
			Instantiate(bulletHolePrefab, spawnPos, Quaternion.LookRotation(hitPoint.normal));
		
	}

	public bool CanShoot()
	{
		if (!player.IsReloading && Time.time > nextFireTime && bulletsLeftInMagazine > 0 && canShoot)
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
			if (inventory.CheckItemCount(weaponData.ammoType) >= weaponData.magazineCapacity)
			{
				return true;
			}
		}

		canShoot = true;
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
		canShoot = false;

		yield return new WaitForSeconds(0.2f);

		float reloadSpeed = 1f / weaponData.reloadTime;
		float percent = 0;

		while (percent < 1)
		{
			percent += Time.deltaTime * reloadSpeed;
			yield return null;
		}

		player.IsReloading = false;
		inventory.RemoveItem(weaponData.ammoType, weaponData.magazineCapacity);
		canShoot = true;
		bulletsLeftInMagazine = weaponData.magazineCapacity;
	}
}
