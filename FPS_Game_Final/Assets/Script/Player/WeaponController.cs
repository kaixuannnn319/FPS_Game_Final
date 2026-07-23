using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private InventoryController inventory;
    private PlayerHealth playerHealth;
    private Camera playerCamera;
    [Header("Knife")]
    [SerializeField] private float knifeRange = 2f;

    [Header("Weapon Models")]
    [SerializeField] private GameObject knifeModel;
    [SerializeField] private GameObject wandLevel1Model;
    [SerializeField] private GameObject wandLevel2Model;
    [SerializeField] private GameObject wandLevel3Model;

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
        UpdateWeaponModel();
    }

    void Update()
    {
        WeaponSwitch();
        Attack();

    }

    private void WeaponSwitch()
    {
        // 1 = Knife
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (inventory.SwitchWeapon(WeaponType.Knife))
            {
                UpdateWeaponStats();
                UpdateWeaponModel();
            }
        }

        // 2 = Wand Level 1
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (inventory.SwitchWeapon(WeaponType.WandLevel1))
            {
                UpdateWeaponStats();
                UpdateWeaponModel();
            }
        }

        // 3 = Wand Level 2
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (inventory.SwitchWeapon(WeaponType.WandLevel2))
            {
                UpdateWeaponStats();
                UpdateWeaponModel();
            }
        }

        // 4 = Wand Level 3
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (inventory.SwitchWeapon(WeaponType.WandLevel3))
            {
                UpdateWeaponStats();
                UpdateWeaponModel();
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

    private void UpdateWeaponModel()
    {
        Debug.Log("UpdateWeaponModel : " + currentWeaponType);

        knifeModel.SetActive(false);
        wandLevel1Model.SetActive(false);

        switch (currentWeaponType)
        {
            case WeaponType.Knife:

                Debug.Log("Show Knife");
                knifeModel.SetActive(true);
                break;

            case WeaponType.WandLevel1:

                Debug.Log("Show Wand1");
                wandLevel1Model.SetActive(true);
                break;

            case WeaponType.WandLevel2:

                Debug.Log("Show Wand2");
                wandLevel2Model.SetActive(true);
                break;

            case WeaponType.WandLevel3:

                Debug.Log("Show Wand3");
                wandLevel3Model.SetActive(true);
                break;
        }
    }
    public void RefreshWeapon()
    {
        UpdateWeaponStats();
        UpdateWeaponModel();
    }

    private void Attack()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (Time.time < nextFireTime)
            return;

        nextFireTime = Time.time + fireCooldown;

        switch (currentWeaponType)
        {
            case WeaponType.Knife:
                KnifeAttack();
                break;

            case WeaponType.WandLevel1:
                WandLevel1Attack();
                break;

            case WeaponType.WandLevel2:
                WandLevel2Attack();
                break;

            case WeaponType.WandLevel3:
                WandLevel3Attack();
                break;
        }
    }
    private void KnifeAttack()
    {
        Debug.Log("Knife Attack");

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward);

        Debug.DrawRay(
            playerCamera.transform.position,
            playerCamera.transform.forward * knifeRange,
            Color.red,
            1f);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, knifeRange))
        {
            EnemyBase enemy = hit.collider.GetComponent<EnemyBase>();

            if (enemy != null)
            {
                enemy.TakeDamage(currentDamage);

                Debug.Log("Hit Enemy");
            }
        }
    }
    private void WandLevel1Attack()
    {
        Debug.Log("Wand Level 1 Attack");
    }
    private void WandLevel2Attack()
    {
        Debug.Log("Wand Level 2 Attack");
    }
    private void WandLevel3Attack()
    {
        Debug.Log("Wand Level 3 Attack");
    }

}