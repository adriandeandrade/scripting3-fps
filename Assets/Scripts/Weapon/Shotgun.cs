using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
	[Header("Shotgun Configuration")]
	[SerializeField] private int bearingCount = 6;
	[SerializeField] private float spreadRadius;
	[SerializeField] private float spreadDepth = 10f; // The height of the radius cone.
	[SerializeField] private GameObject debugObject;

	protected override void Start()
	{
		base.Start();
	}

	protected override void ShootRay()
	{
		RaycastHit hit;
		List<RaycastHit> hitPoints = new List<RaycastHit>();
		List<GameObject> objectsHit = new List<GameObject>();

		Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));

		for (int i = 0; i < bearingCount; i++)
		{
			Vector3 direction = Random.insideUnitCircle * spreadRadius;
			direction.z = spreadDepth;
			direction = transform.TransformDirection(direction.normalized);

			if (debug)
			{
				Debug.DrawRay(rayOrigin, cam.transform.forward * 300, Color.green);
			}

			if (Physics.Raycast(rayOrigin, direction, out hit, weaponData.fireRange, ignoreMask))
			{
				IDamageable damageable = hit.collider.GetComponent<IDamageable>();

				if (damageable != null)
				{
					damageable.TakeDamage(weaponData.damageAmount / bearingCount);
				}

				if (debug)
				{
					Instantiate(debugObject, hit.point, Quaternion.identity);
				}

				objectsHit.Add(hit.collider.gameObject);
				hitPoints.Add(hit);
				//Debug.Log("Object hit: " + hit.collider.name);

			}
		}

		foreach (GameObject objectHit in objectsHit)
		{
			Rigidbody otherRigidBody = objectHit.GetComponent<Rigidbody>();

			if (otherRigidBody != null)
			{
				Vector3 knockbackDirection = otherRigidBody.transform.position - transform.position;
				otherRigidBody.AddForce(knockbackDirection * 0.5f, ForceMode.Impulse);
			}
		}

		SpawnBulletHole(hitPoints);

		animator.SetTrigger("Shoot");
	}

	public void OnPumpAnimationFinished()
	{
		canShoot = true;
	}
}
