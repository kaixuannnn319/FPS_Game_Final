using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;

    private void Start()
    {
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth is not assigned.");
            return;
        }

        if (healthSlider == null)
        {
            Debug.LogError("Health Slider is not assigned.");
            return;
        }

        // Slider will use values from 0 to 1
        healthSlider.minValue = 0f;
        healthSlider.maxValue = 1f;

        playerHealth.OnHealthChange.AddListener(UpdateHealthUI);

        // Initialize UI
        UpdateHealthUI(
            playerHealth.GetCurrentHealth(),
            playerHealth.GetMaxHealth()
        );
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (maxHealth <= 0)
        {
            healthSlider.value = 0f;
            healthText.text = "0/0";
            return;
        }

        healthSlider.value = (float)currentHealth / maxHealth;
        healthText.text = $"{currentHealth}/{maxHealth}";
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChange.RemoveListener(UpdateHealthUI);
    }
}