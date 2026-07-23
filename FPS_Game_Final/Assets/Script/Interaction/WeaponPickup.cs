using UnityEngine;

public class WeaponPickup : MonoBehaviour, IInteractable
{
    [SerializeField]
    private WeaponType weaponType;

    public void Interact(GameObject player)
    {
        InventoryController inventory = player.GetComponent<InventoryController>();

        if (inventory == null)
        {
            Debug.LogError("InventoryController not found on Player!");
            return;
        }

        inventory.UnlockWeapon(weaponType);
        inventory.SwitchWeapon(WeaponType.Knife);

        WeaponController weaponController =
        player.GetComponent<WeaponController>();

        if (weaponController != null)
        {
            weaponController.RefreshWeapon();
        }


        Debug.Log("Picked up weapon : " + weaponType);

        Destroy(gameObject);
    }
}