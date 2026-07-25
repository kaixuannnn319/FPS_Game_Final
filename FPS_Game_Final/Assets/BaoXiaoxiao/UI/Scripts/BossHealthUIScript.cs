using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthUIScript : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text bossNameText;
    [SerializeField] private Image bossBarFill;
    [SerializeField] private TMP_Text bossHPText;

    [Header("Display Settings")]
    [Tooltip("正式整合时可以勾选，让 Boss 血条在游戏开始时隐藏。")]
    [SerializeField] private bool hideOnStart;

    [Header("Temporary Test Data")]
    [SerializeField] private string testBossName = "CYCLOPS";
    [SerializeField] private int testMaxHealth = 500;
    [SerializeField] private int testCurrentHealth = 500;

    private void Awake()
    {
        if (hideOnStart)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Boss 战开始时显示血条，并设置 Boss 名称和血量。
    /// </summary>
    public void ShowBoss(string bossName, int currentHealth, int maxHealth)
    {
        gameObject.SetActive(true);

        if (bossNameText != null)
        {
            bossNameText.text = bossName.ToUpperInvariant();
        }

        UpdateHealth(currentHealth, maxHealth);
    }

    /// <summary>
    /// Boss 血量发生变化时更新 UI。
    /// </summary>
    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        int safeMaxHealth = Mathf.Max(1, maxHealth);
        int safeCurrentHealth =
            Mathf.Clamp(currentHealth, 0, safeMaxHealth);

        if (bossBarFill != null)
        {
            bossBarFill.fillAmount =
                (float)safeCurrentHealth / safeMaxHealth;
        }

        if (bossHPText != null)
        {
            bossHPText.text =
                $"{safeCurrentHealth} / {safeMaxHealth}";
        }
    }

    /// <summary>
    /// Boss 死亡或战斗结束时隐藏血条。
    /// </summary>
    public void HideBoss()
    {
        gameObject.SetActive(false);
    }

    // 以下功能只是方便我们测试 UI。

    [ContextMenu("Test: Show Boss")]
    private void TestShowBoss()
    {
        testCurrentHealth = Mathf.Clamp(
            testCurrentHealth,
            0,
            Mathf.Max(1, testMaxHealth)
        );

        ShowBoss(
            testBossName,
            testCurrentHealth,
            testMaxHealth
        );
    }

    [ContextMenu("Test: Take 100 Damage")]
    private void TestTakeDamage()
    {
        testCurrentHealth =
            Mathf.Max(0, testCurrentHealth - 100);

        ShowBoss(
            testBossName,
            testCurrentHealth,
            testMaxHealth
        );
    }

    [ContextMenu("Test: Reset Health")]
    private void TestResetHealth()
    {
        testCurrentHealth = Mathf.Max(1, testMaxHealth);

        ShowBoss(
            testBossName,
            testCurrentHealth,
            testMaxHealth
        );
    }

    [ContextMenu("Test: Hide Boss")]
    private void TestHideBoss()
    {
        HideBoss();
    }
}