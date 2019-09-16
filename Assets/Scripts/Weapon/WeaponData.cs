using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapons/New Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public AudioClip shootSound;
    public int magazineCapacity;
    public int damageAmount;
    public float fireRate;
    public float fireRange;
    public float cycleTime;
    public float reloadTime;
}
