using UnityEngine;
using TMPro;

public class WeaponAmmoUI : MonoBehaviour
{
	// Inspector Fields
	public TextMeshProUGUI ammoLeftInMagText;
	public TextMeshProUGUI ammoLeftInInventoryText;

	public void UpdateWeaponAmmoUI(Weapon weapon, Item item)
	{
		int ammoLeftInWeapon = weapon.BulletsLeftInMagazine;
		int ammoLeftInInventory = Toolbox.instance.GetInventoryManager().inventory.CheckItemCount(weapon.WeaponData.ammoType);

		ammoLeftInInventoryText.SetText(ammoLeftInInventory.ToString());
		ammoLeftInMagText.SetText(ammoLeftInWeapon.ToString());
	}

	public void UpdateWeaponAmmoUI(Weapon weapon)
	{
		if (Toolbox.instance.GetInventoryManager().inventory != null)
		{
			int ammoLeftInWeapon = weapon.BulletsLeftInMagazine;
			int ammoLeftInInventory = Toolbox.instance.GetInventoryManager().inventory.CheckItemCount(weapon.WeaponData.ammoType);

			ammoLeftInInventoryText.SetText(ammoLeftInInventory.ToString());
			ammoLeftInMagText.SetText(ammoLeftInWeapon.ToString());
		}
		else
		{
			Debug.Log("Inventory not found.");
		}
	}
}
