using UnityEngine;

public enum WeaponType
{
    Knife,
    Wand
}

public class WeaponPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private WeaponType weaponType;

    public void Interact(GameObject player)
    {
        PlayerInventoryStub.Instance.CollectWeapon(weaponType);
        Destroy(gameObject);
    }
}