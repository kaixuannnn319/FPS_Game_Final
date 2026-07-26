using UnityEngine;

public class BuffPickup : MonoBehaviour, IInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Interact(GameObject player)
    {
        InventoryController inventory = player.GetComponent<InventoryController>();

        if (inventory == null)
        {
            Debug.LogError("InventoryController not found on Player!");
            return;
        }

        bool wasAdded = inventory.AddBuff();

        if (wasAdded)
        {
            Debug.Log("Picked up: Buff");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Cannot pick up Buff - already at max capacity");
        }
    }
}