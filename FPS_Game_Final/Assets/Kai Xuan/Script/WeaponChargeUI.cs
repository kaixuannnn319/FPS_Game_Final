using UnityEngine;

public class WeaponChargeUI : MonoBehaviour
{
    [SerializeField] private InventoryController inventory;
    [SerializeField] private WeaponSwitchUIController weaponUI;

    private void Update()
    {
        WeaponType weapon = inventory.GetCurrentWeaponType();

        switch (weapon)
        {
            case WeaponType.WandLevel1:
                weaponUI.SetChargeValue(inventory.GetLevel1Energy(), 100f);
                break;

            case WeaponType.WandLevel2:
                weaponUI.SetChargeValue(inventory.GetLevel2Energy(), 100f);
                break;

            case WeaponType.WandLevel3:
                weaponUI.SetChargeValue(inventory.GetLevel3Energy(), 100f);
                break;
        }
    }
}