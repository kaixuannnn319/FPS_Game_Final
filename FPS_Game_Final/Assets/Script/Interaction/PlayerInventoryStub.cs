using UnityEngine;
using System.Collections.Generic;

public class PlayerInventoryStub : MonoBehaviour
{
    public static PlayerInventoryStub Instance;
    private List<string> collectedKeys = new List<string>();

    // ADDED - weapon tracking
    private bool hasKnife = false;
    private bool hasWand = false;

    private void Awake()
    {
        Instance = this;
    }

    public void CollectKey(string keyID)
    {
        collectedKeys.Add(keyID);
        Debug.Log("Collected key: " + keyID);
    }

    public bool HasKey(string keyID)
    {
        return collectedKeys.Contains(keyID);
    }

    // ADDED - weapon methods
    public void CollectWeapon(WeaponType type)
    {
        if (type == WeaponType.Knife)
        {
            hasKnife = true;
        }
        else if (type == WeaponType.Wand)
        {
            hasWand = true;
        }

        Debug.Log("Weapon added to inventory: " + type);
    }

    public bool HasWeapon(WeaponType type)
    {
        if (type == WeaponType.Knife) return hasKnife;
        if (type == WeaponType.Wand) return hasWand;
        return false;
    }
}