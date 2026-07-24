using UnityEngine;

public enum HealingItemType
{
    Bandage,
    Elixir
}

public class HealthPotionPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private HealingItemType healingItemType;

    public void Interact(GameObject player)
    {
        InventoryController inventory = player.GetComponent<InventoryController>();
        if (inventory == null)
        {
            Debug.LogError("InventoryController not found on Player!");
            return;
        }

        bool wasAdded = false;

        if (healingItemType == HealingItemType.Bandage)
        {
            wasAdded = inventory.AddBandage();
        }
        else if (healingItemType == HealingItemType.Elixir)
        {
            wasAdded = inventory.AddElixir();
        }

        if (wasAdded)
        {
            Debug.Log("Picked up: " + healingItemType);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Cannot pick up " + healingItemType + " - already at max capacity");
        }
    }
}
