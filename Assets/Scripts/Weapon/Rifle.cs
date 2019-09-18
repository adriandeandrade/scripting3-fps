using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    protected override void ShootRay()
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
                otherRigidBody.AddForceAtPosition(knockbackDirection * 5f, hit.point, ForceMode.Impulse);
            }

            SpawnBulletHole(hit);

    		Debug.Log("Object hit: " + hit.collider.name);
		}
    }
}
