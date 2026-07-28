using UnityEngine;

public class WeaponPickup : MonoBehaviour, IInteractable
{
    [SerializeField]
    private WeaponType weaponType;

    public void Interact(GameObject player)
    {
        Debug.Log("WeaponPickup Interact!");
        InventoryController inventory = player.GetComponent<InventoryController>();

        if (inventory == null)
        {
            Debug.LogError("InventoryController not found on Player!");
            return;
        }

        inventory.UnlockWeapon(weaponType);
        inventory.SwitchWeapon(weaponType);

        WeaponController weaponController =
        player.GetComponent<WeaponController>();

        if (weaponController != null)
        {
            weaponController.RefreshWeapon();
        }


        Debug.Log("Picked up weapon : " + weaponType);
        Debug.Log("Destroy : " + gameObject.name);
        player.GetComponent<PlayerPickupAudio>()?.PlayPickupSound();
        Destroy(gameObject);
    }
}