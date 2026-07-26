using UnityEngine;

public class WeaponAmmoHUDUI : MonoBehaviour
{
    [SerializeField] private InventoryController inventory;
    [SerializeField] private WeaponAmmoUIController uiController;

    private void Start()
    {
        inventory.OnInventoryChanged.AddListener(UpdateHUD);

        UpdateHUD();
    }

    private void OnDestroy()
    {
        if (inventory != null)
            inventory.OnInventoryChanged.RemoveListener(UpdateHUD);
    }

    private void UpdateHUD()
    {
        uiController.UpdateAmmoAvailability(
            inventory.HasLevel1Weapon(),
            inventory.HasLevel2Weapon(),
            inventory.HasLevel3Weapon());

        uiController.UpdateAmmoCount(
            inventory.GetLevel1ReserveEnergy(),
            inventory.GetLevel2ReserveEnergy(),
            inventory.GetLevel3ReserveEnergy());
    }
}