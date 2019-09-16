using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
	[Header("Shotgun Configuration")]
	[SerializeField] private int bearingCount = 6;
	[SerializeField] private float spreadRadius;
	[SerializeField] private float z = 10f;
	[SerializeField] private GameObject debugObject;
	[SerializeField] private bool debug;

	public override void ShootRay()
	{
		RaycastHit hit;
		List<GameObject> objectsHit = new List<GameObject>();

		Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));

		for (int i = 0; i < bearingCount; i++)
		{
			Vector3 direction = Random.insideUnitCircle * spreadRadius;
			direction.z = z;
			direction = transform.TransformDirection(direction.normalized);

			if (Physics.Raycast(rayOrigin, direction, out hit, weaponData.fireRange, ignoreMask))
			{
				if (debug)
				{
					Instantiate(debugObject, hit.point, Quaternion.identity);
				}

				objectsHit.Add(hit.collider.gameObject);
				Debug.Log("Object hit: " + hit.collider.name);
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
	}
}
