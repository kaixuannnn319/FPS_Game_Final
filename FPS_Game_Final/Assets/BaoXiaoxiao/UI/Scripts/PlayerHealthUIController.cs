using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthUIController : MonoBehaviour
{
    [Header("Health UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;

    [Header("Temporary Keyboard Test")]
    [Tooltip("测试阶段勾选。正式连接玩家系统后取消勾选。")]
    [SerializeField] private bool enableKeyboardTest = true;

    [SerializeField] private float testMaximumHealth = 100f;
    [SerializeField] private float testCurrentHealth = 100f;
    [SerializeField] private float testDamageAmount = 10f;
    [SerializeField] private float testHealAmount = 10f;

    private void Awake()
    {
        if (healthSlider == null)
        {
            Debug.LogError(
                "PlayerHealthUIController: Health Slider is not assigned."
            );

            enabled = false;
            return;
        }

        healthSlider.minValue = 0f;
        healthSlider.maxValue = 1f;

        SetHealth(testCurrentHealth, testMaximumHealth);
    }

    private void Update()
    {
        if (!enableKeyboardTest)
        {
            return;
        }

        // K：测试扣血
        if (Input.GetKeyDown(KeyCode.K))
        {
            testCurrentHealth -= testDamageAmount;

            testCurrentHealth = Mathf.Clamp(
                testCurrentHealth,
                0f,
                testMaximumHealth
            );

            SetHealth(
                testCurrentHealth,
                testMaximumHealth
            );
        }

        // L：测试回血
        if (Input.GetKeyDown(KeyCode.L))
        {
            testCurrentHealth += testHealAmount;

            testCurrentHealth = Mathf.Clamp(
                testCurrentHealth,
                0f,
                testMaximumHealth
            );

            SetHealth(
                testCurrentHealth,
                testMaximumHealth
            );
        }
    }

    /// <summary>
    /// 玩家系统在生命值变化后调用此方法。
    /// </summary>
    public void SetHealth(
        float currentHealth,
        float maximumHealth
    )
    {
        if (healthSlider == null)
        {
            return;
        }

        if (maximumHealth <= 0f)
        {
            healthSlider.value = 0f;

            if (healthText != null)
            {
                healthText.text = "0/0";
            }

            return;
        }

        currentHealth = Mathf.Clamp(
            currentHealth,
            0f,
            maximumHealth
        );

        healthSlider.value =
            currentHealth / maximumHealth;

        if (healthText != null)
        {
            healthText.text =
                $"{Mathf.CeilToInt(currentHealth)}/" +
                $"{Mathf.CeilToInt(maximumHealth)}";
        }
    }

    /// <summary>
    /// 直接使用0到1的比例更新生命条。
    /// </summary>
    public void SetHealthNormalized(float normalizedHealth)
    {
        if (healthSlider == null)
        {
            return;
        }

        healthSlider.value =
            Mathf.Clamp01(normalizedHealth);
    }
}