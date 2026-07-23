using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private InventoryController inventory;
    private PlayerHealth playerHealth;
    private Camera playerCamera;

    [Header("Weapon Stats")]
    private WeaponLevel currentWeapon;

    // Current Weapon Stats
    private int currentDamage;
    private float currentEnergyCost;
    private float fireCooldown;

    // Fire Timer
    private float nextFireTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory = GetComponent<InventoryController>();
        playerHealth = GetComponent<PlayerHealth>();
        playerCamera = Camera.main;
        UpdateWeaponStats();
    }

    // Update is called once per frame
    void Update()
    {
        WeaponSwitch();
    }
    private void WeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (inventory.SwitchWeapon(WeaponLevel.Knife))
            {
                UpdateWeaponStats();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (inventory.SwitchWeapon(WeaponLevel.Level1Weapon))
            {
                UpdateWeaponStats();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (inventory.SwitchWeapon(WeaponLevel.Level2Weapon))
            {
                UpdateWeaponStats();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (inventory.SwitchWeapon(WeaponLevel.Level3Weapon))
            {
                UpdateWeaponStats();
            }
        }
    }
    private void UpdateWeaponStats()
    {
        currentWeapon = inventory.GetCurrentWeapon();

        switch (currentWeapon)
        {
            case WeaponLevel.Knife:

                currentDamage = 5;
                currentEnergyCost = 0;
                fireCooldown = 0;

                break;

            case WeaponLevel.Level1Weapon:

                currentDamage = 20;
                currentEnergyCost = 5;
                fireCooldown = 0.2f;

                break;

            case WeaponLevel.Level2Weapon:

                currentDamage = 35;
                currentEnergyCost = 10;
                fireCooldown = 0.5f;

                break;

            case WeaponLevel.Level3Weapon:

                currentDamage = 50;
                currentEnergyCost = 20;
                fireCooldown = 1f;

                break;

            default:

                currentDamage = 0;
                currentEnergyCost = 0;
                fireCooldown = 0;

                break;
        }
    }
}


