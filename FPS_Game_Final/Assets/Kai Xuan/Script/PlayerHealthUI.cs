using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image healthFill;
    [SerializeField] private TMP_Text healthText;

    private void Start()
    {
        playerHealth.OnHealthChange.AddListener(UpdateHealthUI);

        UpdateHealthUI(
            playerHealth.GetCurrentHealth(),
            playerHealth.GetMaxHealth());
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        healthFill.fillAmount = (float)currentHealth / maxHealth;
        healthText.text = currentHealth + "/" + maxHealth;
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChange.RemoveListener(UpdateHealthUI);
    }
}