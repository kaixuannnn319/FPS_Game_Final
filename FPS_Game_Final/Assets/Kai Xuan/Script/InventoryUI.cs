using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryController inventory;
    [SerializeField] private InventoryUIController ui;

    private void Start()
    {
        inventory.OnInventoryChanged.AddListener(UpdateInventoryUI);

        // Initial display
        UpdateInventoryUI();
    }

    private void OnDestroy()
    {
        if (inventory != null)
            inventory.OnInventoryChanged.RemoveListener(UpdateInventoryUI);
    }

    private void UpdateInventoryUI()
    {
        ui.UpdateInventory(
            inventory.GetBandageCount(),
            inventory.GetElixirCount(),
            inventory.GetBuffCount(),

            inventory.GetLevel1ReserveEnergy(),
            inventory.GetLevel2ReserveEnergy(),
            inventory.GetLevel3ReserveEnergy(),

            inventory.HasKnife(),
            inventory.HasLevel1Weapon(),
            inventory.HasLevel2Weapon(),
            inventory.HasLevel3Weapon()
        );
    }
}