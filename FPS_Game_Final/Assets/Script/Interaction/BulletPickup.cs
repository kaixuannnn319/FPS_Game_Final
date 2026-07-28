using UnityEngine;

public enum BulletLevel
{
    Level1,
    Level2,
    Level3
}

public class BulletPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private BulletLevel bulletLevel;

    public void Interact(GameObject player)
    {
        InventoryController inventory = player.GetComponent<InventoryController>();

        if (inventory == null)
        {
            Debug.LogError("InventoryController not found on Player!");
            return;
        }

        bool wasAdded = false;

        switch (bulletLevel)
        {
            case BulletLevel.Level1:
                wasAdded = inventory.AddLevel1ReserveEnergy();
                break;

            case BulletLevel.Level2:
                wasAdded = inventory.AddLevel2ReserveEnergy();
                break;

            case BulletLevel.Level3:
                wasAdded = inventory.AddLevel3ReserveEnergy();
                break;
        }

        if (wasAdded)
        {
            Debug.Log("Picked up: " + bulletLevel + " bullets");
            player.GetComponent<PlayerPickupAudio>()?.PlayPickupSound();
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Cannot pick up " + bulletLevel + " bullets - reserve already full");
        }
    }
}