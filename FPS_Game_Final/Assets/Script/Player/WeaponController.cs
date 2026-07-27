using UnityEngine;
using System.Collections;
using UnityEngine.Events;
public class WeaponController : MonoBehaviour
{
    private InventoryController inventory;
    private PlayerHealth playerHealth;
    private Camera playerCamera;
    private Animator animator;

    public UnityEvent<WeaponType> OnWeaponChanged = new UnityEvent<WeaponType>();


    [Header("Knife")]
    [SerializeField] private float knifeRange = 2f;

    [Header("Weapon Models")]
    [SerializeField] private GameObject knifeModel;
    [SerializeField] private GameObject wandLevel1Model;
    [SerializeField] private GameObject wandLevel2Model;
    [SerializeField] private GameObject wandLevel3Model;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletLevel1;
    [SerializeField] private GameObject bulletLevel2;
    [SerializeField] private GameObject bulletLevel3;
    [SerializeField] private GameObject knifeHitEffect;
    [SerializeField] private Transform knifePoint;
    [SerializeField] private Transform firePoint;
    [Header("Current Weapon")]
    private WeaponType currentWeaponType;

    [Header("Weapon Stats")]
    private int currentDamage;
    private float currentEnergyCost;
    private float fireCooldown;


    private GameObject currentBulletPrefab;

    [Header("Fire Timer")]
    private float nextFireTime;

    private bool isAiming = false;
    [Header("Aim Down Sight")]
    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private float aimFOV = 35f;
    [SerializeField] private float aimSpeed = 10f;

    [Header("Damage Buff")]
    [SerializeField] private float buffMultiplier = 2f;
    [SerializeField] private float buffDuration = 15f;

    [SerializeField] private InventoryToggle inventoryToggle;

    [SerializeField] private BuffStatusUIScript buffStatusUI;

    [SerializeField] private StoryUIController storyUI;

    [SerializeField] private DialogueUI dialogueUI;

    [SerializeField] private ClueDocumentUI clueUI;

    private bool isBuffActive = false;

    public bool IsBuffActive => isBuffActive;

    void Start()
    {
        inventory = GetComponent<InventoryController>();
        playerHealth = GetComponent<PlayerHealth>();
        playerCamera = Camera.main;
        animator = GetComponentInChildren<Animator>();
        playerCamera.fieldOfView = normalFOV;

        UpdateWeaponStats();
        UpdateWeaponModel();

        OnWeaponChanged?.Invoke(currentWeaponType);
    }

    void Update()
    {
        if (inventoryToggle != null && inventoryToggle.IsOpen)
            return;

        if (storyUI != null && storyUI.IsStoryOpen)
            return;

        if (dialogueUI != null && dialogueUI.IsDialogueOpen)
            return;

        if (clueUI != null)
        {
            Debug.Log("WeaponController ClueUI = " + clueUI.gameObject.name);
            Debug.Log("WeaponController sees IsOpen = " + clueUI.IsClueOpen);

            if (clueUI.IsClueOpen)
            {
                Debug.Log("Weapon blocked");
                return;
            }
        }
        else
        {
            Debug.LogError("WeaponController has NO ClueUI reference!");
        }

        WeaponSwitch();
        Attack();
        Aim();

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadCurrentWeapon();
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            UseDamageBuff();
        }
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

                OnWeaponChanged?.Invoke(currentWeaponType);
            }
        }

        // 2 = Wand Level 1
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (inventory.SwitchWeapon(WeaponType.WandLevel1))
            {
                UpdateWeaponStats();
                UpdateWeaponModel();

                OnWeaponChanged?.Invoke(currentWeaponType);
            }
        }

        // 3 = Wand Level 2
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (inventory.SwitchWeapon(WeaponType.WandLevel2))
            {
                UpdateWeaponStats();
                UpdateWeaponModel();

                OnWeaponChanged?.Invoke(currentWeaponType);
            }
        }

        // 4 = Wand Level 3
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (inventory.SwitchWeapon(WeaponType.WandLevel3))
            {
                UpdateWeaponStats();
                UpdateWeaponModel();

                OnWeaponChanged?.Invoke(currentWeaponType);
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
                currentBulletPrefab = null;

                break;

            case WeaponType.Knife:

                currentDamage = 5;
                currentEnergyCost = 0;
                fireCooldown = 0;
                currentBulletPrefab = null;

                break;

            case WeaponType.WandLevel1:

                currentDamage = 5;
                currentEnergyCost = 5;
                fireCooldown = 0.2f;
                currentBulletPrefab = bulletLevel1;

                break;

            case WeaponType.WandLevel2:

                currentDamage = 20;
                currentEnergyCost = 10;
                fireCooldown = 0.5f;
                currentBulletPrefab = bulletLevel2;

                break;

            case WeaponType.WandLevel3:

                currentDamage = 50;
                currentEnergyCost = 20;
                fireCooldown = 1.0f;
                currentBulletPrefab = bulletLevel3;

                break;
        }

        Debug.Log(
            $"Current Weapon : {currentWeaponType} | Damage : {currentDamage} | Energy Cost : {currentEnergyCost}");
    }

    private void UpdateWeaponModel()
    {
        Debug.Log("UpdateWeaponModel : " + currentWeaponType);

        if (knifeModel != null)
            knifeModel.SetActive(false);

        if (wandLevel1Model != null)
            wandLevel1Model.SetActive(false);

        if (wandLevel2Model != null)
            wandLevel2Model.SetActive(false);

        if (wandLevel3Model != null)
            wandLevel3Model.SetActive(false);

        switch (currentWeaponType)
        {
            case WeaponType.Knife:

                if (knifeModel != null)
                    knifeModel.SetActive(true);

                break;

            case WeaponType.WandLevel1:

                if (wandLevel1Model != null)
                    wandLevel1Model.SetActive(true);

                break;

            case WeaponType.WandLevel2:

                if (wandLevel2Model != null)
                    wandLevel2Model.SetActive(true);

                break;

            case WeaponType.WandLevel3:

                if (wandLevel3Model != null)
                    wandLevel3Model.SetActive(true);

                break;
        }
    }
    public void RefreshWeapon()
    {
        UpdateWeaponStats();
        UpdateWeaponModel();

        OnWeaponChanged?.Invoke(currentWeaponType);
    }

    private void Attack()
    {
        if (!Input.GetMouseButtonDown(0))
            return;
        Debug.Log("Current Weapon = " + currentWeaponType);

        if (Time.time < nextFireTime)
            return;

        if (currentWeaponType != WeaponType.Knife)
        {
            if (!inventory.UseEnergy(currentWeaponType, currentEnergyCost))
            {
                Debug.Log("Not enough Energy!");
                return;
            }
        }

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
        animator.SetTrigger("KnifeAttack");

        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f));

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, knifeRange))
        {
            Debug.Log("Knife Hit!");

            EnemyBase enemy = hit.collider.GetComponentInParent<EnemyBase>();

            if (enemy != null)
            {
                int damage = currentDamage;

                if (isBuffActive)
                {
                    damage = Mathf.RoundToInt(currentDamage * buffMultiplier);
                }

                enemy.TakeDamage(damage);

                if (knifeHitEffect != null)
                {
                    Instantiate(
                        knifeHitEffect,
                        hit.point,
                        Quaternion.LookRotation(hit.normal));
                }
            }
        }
    }
    private void WandLevel1Attack()
    {
        animator.SetTrigger("WandAttack");

    }

    private void WandLevel2Attack()
    {
        animator.SetTrigger("WandAttack");

    }

    private void WandLevel3Attack()
    {
        animator.SetTrigger("WandAttack");

    }
    public void ShootBullet()
    {
        Debug.Log("FirePoint = " + firePoint);

        if (firePoint == null)
        {
            Debug.LogError("FirePoint is Missing!");
            return;
        }

        Debug.Log(currentBulletPrefab);
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit hit;

        Vector3 targetPoint;


        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100f);
        }

        Vector3 direction =
            (targetPoint - firePoint.position).normalized;

        GameObject bullet = Instantiate(
        currentBulletPrefab,
        firePoint.position,
        Quaternion.LookRotation(direction));

        BulletController bulletController = bullet.GetComponent<BulletController>();

        if (bulletController != null)
        {
            int damage = currentDamage;

            if (isBuffActive)
            {
                damage = Mathf.RoundToInt(currentDamage * buffMultiplier);
            }

            bulletController.damage = damage;
        }

        // Ignore collision with the current weapon
        Collider bulletCollider = bullet.GetComponent<Collider>();

        GameObject activeWeapon = null;

        switch (currentWeaponType)
        {
            case WeaponType.WandLevel1:
                activeWeapon = wandLevel1Model;
                break;

            case WeaponType.WandLevel2:
                activeWeapon = wandLevel2Model;
                break;

            case WeaponType.WandLevel3:
                activeWeapon = wandLevel3Model;
                break;
        }

        if (activeWeapon != null && bulletCollider != null)
        {
            foreach (Collider weaponCollider in activeWeapon.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(bulletCollider, weaponCollider);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (knifePoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(knifePoint.position, knifeRange);
    }

    private void Aim()
    {
        if (currentWeaponType == WeaponType.Knife)
            return;

        if (Input.GetMouseButtonDown(1))
        {
            isAiming = !isAiming;
        }

        float targetFOV = isAiming ? aimFOV : normalFOV;

        playerCamera.fieldOfView = Mathf.Lerp(
            playerCamera.fieldOfView,
            targetFOV,
            Time.deltaTime * aimSpeed);
    }

    private void ReloadCurrentWeapon()
    {
        if (currentWeaponType == WeaponType.Knife)
            return;

        if (!inventory.ReloadEnergy(currentWeaponType))
        {
            Debug.Log("No Reserve Energy!");
        }
    }

    private void UseDamageBuff()
    {
        if (isBuffActive)
        {
            Debug.Log("Buff already active!");
            return;
        }

        if (!inventory.UseBuff())
        {
            Debug.Log("No Buff!");
            return;
        }

        if (buffStatusUI != null)
        {
            buffStatusUI.StartBuffCountdown(buffDuration);
        }

        StartCoroutine(DamageBuffRoutine());
    }
    private IEnumerator DamageBuffRoutine()
    {
        isBuffActive = true;

        Debug.Log("Damage Buff Activated!");
        Debug.Log("Current Damage x2");

        yield return new WaitForSeconds(buffDuration);

        isBuffActive = false;

        inventory.OnInventoryChanged?.Invoke();

        Debug.Log("Damage Buff Ended!");
    }
}