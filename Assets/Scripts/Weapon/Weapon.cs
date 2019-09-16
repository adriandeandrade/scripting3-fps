using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	// Inspector Fields
	[Header("Weapon Configuration")]
	[SerializeField] protected WeaponData weaponData;
	[SerializeField] protected LayerMask ignoreMask;

	private enum WeaponStates { READY, RELOAD, CYCLING, NOAMMO };
	private WeaponStates weaponState = WeaponStates.READY;

	// Private Variables
	private int bulletsLeftInMagazine;
	public int bulletsLeftBeforeReload;
	private float nextFireTime;

	// Components
	protected Camera cam;
	private Player player;

	private void Awake()
	{
		cam = Camera.main;
		player = FindObjectOfType<Player>(); // Get in GameManager and use that as reference to player. [Toolbox.instance.GetGameManager.PlayerRef]
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

	public virtual void ShootRay()
	{
		RaycastHit hit;

		Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
		Debug.DrawRay(rayOrigin, cam.transform.forward * 300, Color.green);

		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weaponData.fireRange, ignoreMask))
		{
            Rigidbody otherRigidBody = hit.collider.GetComponent<Rigidbody>();

            if(otherRigidBody != null)
            {
                Vector3 knockbackDirection = otherRigidBody.transform.position - transform.position;
                otherRigidBody.AddForce(knockbackDirection * 5f, ForceMode.Impulse);
            }

    		Debug.Log("Object hit: " + hit.collider.name);
		}
	}

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
