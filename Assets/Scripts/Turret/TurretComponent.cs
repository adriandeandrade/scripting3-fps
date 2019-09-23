using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretComponent : BaseDamageable
{
    public override void TakeDamage(float amount)
    {
        Turret1 mainComponent = GetComponentInParent<Turret1>();
        mainComponent.TakeDamage(amount);
        Debug.Log("Turret part hit: " + gameObject.name);
    }
}
