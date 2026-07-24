using UnityEngine;

public class SacredAltar : MonoBehaviour, IInteractable
{
    [SerializeField] private int requiredSealCount = 3;
    [SerializeField] private GameObject[] sealDisplayObjects;

    private bool isActivated = false;

    public void Interact(GameObject player)
    {
        if (isActivated) return;

        InventoryController inventory = player.GetComponent<InventoryController>();
        if (inventory == null)
        {
            Debug.LogError("InventoryController not found on Player!");
            return;
        }

        if (inventory.GetRelicCount() >= requiredSealCount)
        {
            isActivated = true;
            Debug.Log("All Sacred Seals placed on the altar!");

            foreach (GameObject sealVisual in sealDisplayObjects)
            {
                if (sealVisual != null)
                    sealVisual.SetActive(true);
            }
        }
        else
        {
            int missing = requiredSealCount - inventory.GetRelicCount();
            Debug.Log("Missing " + missing + " more Sacred Seal(s).");
        }
    }
}