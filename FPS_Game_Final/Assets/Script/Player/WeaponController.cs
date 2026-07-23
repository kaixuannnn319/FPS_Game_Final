using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private InventoryController inventory;
    private PlayerHealth playerHealth;
    private Camera playerCamera;

    [Header("Current Weapon")]
    private WeaponType currentWeaponType;

    [Header("Weapon Stats")]
    private int currentDamage;
    private float currentEnergyCost;
    private float fireCooldown;

    [Header("Fire Timer")]
    private float nextFireTime;

    void Start()
    {
        inventory = GetComponent<InventoryController>();
        playerHealth = GetComponent<PlayerHealth>();
        playerCamera = Camera.main;

        UpdateWeaponStats();
    }

    void Update()
    {
        WeaponSwitch();
    }

    private void WeaponSwitch()
    {
        // 1 = Knife
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (inventory.SwitchWeapon(WeaponType.Knife))
            {
                UpdateWeaponStats();
            }
        }

        // 2 = Wand Level 1
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (inventory.SwitchWeapon(WeaponType.WandLevel1))
            {
                UpdateWeaponStats();
            }
        }

        // 3 = Wand Level 2
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (inventory.SwitchWeapon(WeaponType.WandLevel2))
            {
                UpdateWeaponStats();
            }
        }

        // 4 = Wand Level 3
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (inventory.SwitchWeapon(WeaponType.WandLevel3))
            {
                UpdateWeaponStats();
            }
        }
    }

    private void UpdateWeaponStats()
    {
        currentWeaponType = inventory.GetCurrentWeaponType();

        switch (currentWeaponType)
        {
            case WeaponType.None:

                currentDamage = 0;
                currentEnergyCost = 0;
                fireCooldown = 0;

                break;

            case WeaponType.Knife:

                currentDamage = 5;
                currentEnergyCost = 0;
                fireCooldown = 0;

                break;

            case WeaponType.WandLevel1:

                currentDamage = 20;
                currentEnergyCost = 5;
                fireCooldown = 0.2f;

                break;

            case WeaponType.WandLevel2:

                currentDamage = 35;
                currentEnergyCost = 10;
                fireCooldown = 0.5f;

                break;

            case WeaponType.WandLevel3:

                currentDamage = 50;
                currentEnergyCost = 20;
                fireCooldown = 1.0f;

                break;
        }

        Debug.Log(
            $"Current Weapon : {currentWeaponType} | Damage : {currentDamage} | Energy Cost : {currentEnergyCost}");
    }
}