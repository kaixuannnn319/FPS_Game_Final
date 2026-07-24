using UnityEngine;

public class SacredSealPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private string sealID; // e.g. "Seal1", "Seal2", "Seal3"

    public void Interact(GameObject player)
    {
        InventoryController inventory = player.GetComponent<InventoryController>();
        if (inventory == null)
        {
            Debug.LogError("InventoryController not found on Player!");
            return;
        }

        inventory.AddRelic();
        Debug.Log("Collected Sacred Seal: " + sealID);

        Destroy(gameObject);
    }

    public void Reveal()
    {
        gameObject.SetActive(true);
    }
}