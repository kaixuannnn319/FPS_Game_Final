using UnityEngine;

public class HealingHUDUI : MonoBehaviour
{
    [SerializeField] private InventoryController inventory;
    [SerializeField] private HealingItemUIController uiController;

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
        uiController.UpdateHealingItems(
            inventory.GetBandageCount(),
            inventory.GetElixirCount()
        );
    }
}