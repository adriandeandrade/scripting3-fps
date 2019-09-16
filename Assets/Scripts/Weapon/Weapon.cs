using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
	// Inspector Fields
	[Header("Weapon Configuration")]
	[SerializeField] protected WeaponData weaponData;
	[SerializeField] protected LayerMask ignoreMask;

	protected enum WeaponStates { READY, RELOAD, CYCLING, NOAMMO };
	protected WeaponStates weaponState = WeaponStates.READY;

	// Private Variables
	protected int bulletsLeftInMagazine;
	protected int bulletsLeftBeforeReload;
	protected float nextFireTime;
	protected bool debug;

	// Components
	protected Camera cam;
	protected Player player;
	protected Animator animator;

	private void Awake()
	{
		cam = Camera.main;
		player = FindObjectOfType<Player>(); // Get in GameManager and use that as reference to player. [Toolbox.instance.GetGameManager.PlayerRef]
		animator = GetComponent<Animator>();
	}

	private void Start()
	{
		bulletsLeftInMagazine = weaponData.magazineCapacity;
		nextFireTime = Time.time + weaponData.fireRate;
	}

	private void LateUpdate()
	{
		if (!player.IsReloading && bulletsLeftInMagazine == 0)
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
			Debug.Log("Shot weapon");
		}
	}

	public bool CanShoot()
	{
		if (!player.IsReloading && Time.time > nextFireTime && bulletsLeftInMagazine > 0)
		{
			return true;
		}
		else
		{
			return false;
		}
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
		bulletsLeftInMagazine = weaponData.magazineCapacity;
	}
}
