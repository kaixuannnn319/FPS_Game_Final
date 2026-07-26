using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField]
    private int maxHealth = 100;

    [SerializeField]
    private int currentHealth;
    private InventoryController inventory;

    [SerializeField] private int bandageHealAmount = 30;
    [SerializeField] private int elixirHealAmount = 60;

    [SerializeField] private float bandageUseTime = 1.2f;
    [SerializeField] private float elixirUseTime = 2f;

    private bool isHealing = false;

    //For the UI or other systems to listen to
    public UnityEvent<int,int> OnHealthChange;
    public UnityEvent OnPlayerDeath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChange?.Invoke(currentHealth,maxHealth);
        inventory = GetComponent<InventoryController>();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterPlayer(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(30);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            UseBandage();
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            UseElixir();
        }

    }

    //Damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Current HP : " + currentHealth);

        OnHealthChange?.Invoke(currentHealth, maxHealth);

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    //Heal
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Current HP : " + currentHealth);

        OnHealthChange?.Invoke(currentHealth, maxHealth);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;

        OnHealthChange?.Invoke(currentHealth, maxHealth);

        Debug.Log("Health Reset");
    }

    private void Die()
    {
        Debug.Log("Player Died");

        PlayerController controller = GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        OnPlayerDeath?.Invoke();

        GameManager.Instance.PlayerDied();
    }

    //Getter
    public int GetCurrentHealth()
        { return currentHealth; }

    public int GetMaxHealth()
        { return maxHealth; }

    private void UseBandage()
    {
        if (isHealing)
            return;

        if (currentHealth >= maxHealth)
        {
            Debug.Log("Health is already full!");
            return;
        }

        if (!inventory.UseBandage())
        {
            Debug.Log("No Bandage!");
            return;
        }

        StartCoroutine(BandageRoutine());
    }

    private void UseElixir()
    {
        if (isHealing)
            return;

        if (currentHealth >= maxHealth)
        {
            Debug.Log("Health is already full!");
            return;
        }

        if (!inventory.UseElixir())
        {
            Debug.Log("No Elixir!");
            return;
        }

        StartCoroutine(ElixirRoutine());
    }

    private IEnumerator BandageRoutine()
    {
        isHealing = true;

        Debug.Log("Using Bandage...");

        yield return new WaitForSeconds(bandageUseTime);

        Heal(bandageHealAmount);

        isHealing = false;
    }

    private IEnumerator ElixirRoutine()
    {
        isHealing = true;

        Debug.Log("Using Elixir...");

        yield return new WaitForSeconds(elixirUseTime);

        Heal(elixirHealAmount);

        isHealing = false;
    }
}

