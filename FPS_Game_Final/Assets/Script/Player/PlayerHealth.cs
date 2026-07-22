using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField]
    private int maxHealth = 100;

    private int currentHealth;

    //For the UI or other systems to listen to
    public UnityEvent<int,int> OnHealthChange;
    public UnityEvent OnPlayerDeath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChange?.Invoke(currentHealth,maxHealth);
    }

    // Update is called once per frame
    void Update()
    {


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

    private  void Die()
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
    }

    //Getter
    public int GetCurrentHealth()
        { return currentHealth; }

    public int GetMaxHealth()
        { return maxHealth; }
}

