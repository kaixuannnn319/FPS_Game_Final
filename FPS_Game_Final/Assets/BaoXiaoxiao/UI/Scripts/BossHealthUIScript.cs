using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthUIController : MonoBehaviour
{
    [System.Serializable]
    private class TestBossData
    {
        public string bossName;
        public float maximumHealth;
    }

    [Header("Boss UI References")]
    [SerializeField] private CanvasGroup bossCanvasGroup;
    [SerializeField] private Slider bossHealthSlider;
    [SerializeField] private TMP_Text bossNameText;
    [SerializeField] private TMP_Text bossHPText;

    [Header("Temporary Keyboard Test")]
    [SerializeField] private bool enableKeyboardTest = true;

    [SerializeField]
    private TestBossData[] testBosses =
    {
        new TestBossData
        {
            bossName = "CYCLOPS",
            maximumHealth = 500f
        },
        new TestBossData
        {
            bossName = "DRAGONIDE",
            maximumHealth = 800f
        },
        new TestBossData
        {
            bossName = "EVIL WATCHER",
            maximumHealth = 1200f
        },
        new TestBossData
        {
            bossName = "DEMON LORD",
            maximumHealth = 1500f
        }
    };

    [SerializeField] private float testDamageAmount = 100f;
    [SerializeField] private float testHealAmount = 100f;

    private int testBossIndex = -1;
    private float testCurrentHealth;
    private float testMaximumHealth;

    private void Awake()
    {
        if (bossCanvasGroup == null || bossHealthSlider == null)
        {
            Debug.LogError(
                "BossHealthUIController: Required UI references are missing."
            );

            enabled = false;
            return;
        }

        bossHealthSlider.minValue = 0f;
        bossHealthSlider.maxValue = 1f;

        HideBoss();
    }

    private void Update()
    {
        if (!enableKeyboardTest)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            ShowNextTestBoss();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            testCurrentHealth = Mathf.Clamp(
                testCurrentHealth - testDamageAmount,
                0f,
                testMaximumHealth
            );

            SetBossHealth(
                testCurrentHealth,
                testMaximumHealth
            );
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            testCurrentHealth = Mathf.Clamp(
                testCurrentHealth + testHealAmount,
                0f,
                testMaximumHealth
            );

            SetBossHealth(
                testCurrentHealth,
                testMaximumHealth
            );
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            HideBoss();
        }
    }

    private void ShowNextTestBoss()
    {
        if (testBosses == null || testBosses.Length == 0)
        {
            return;
        }

        testBossIndex++;

        if (testBossIndex >= testBosses.Length)
        {
            testBossIndex = 0;
        }

        TestBossData selectedBoss = testBosses[testBossIndex];

        testMaximumHealth = selectedBoss.maximumHealth;
        testCurrentHealth = testMaximumHealth;

        ShowBoss(
            selectedBoss.bossName,
            testCurrentHealth,
            testMaximumHealth
        );
    }

    public void ShowBoss(
        string bossName,
        float currentHealth,
        float maximumHealth
    )
    {
        SetBossVisible(true);
        SetBossName(bossName);
        SetBossHealth(currentHealth, maximumHealth);
    }

    public void SetBossName(string bossName)
    {
        if (bossNameText == null)
        {
            return;
        }

        bossNameText.text = string.IsNullOrWhiteSpace(bossName)
            ? "BOSS"
            : bossName;
    }

    public void SetBossHealth(
        float currentHealth,
        float maximumHealth
    )
    {
        if (bossHealthSlider == null)
        {
            return;
        }

        if (maximumHealth <= 0f)
        {
            bossHealthSlider.value = 0f;

            if (bossHPText != null)
            {
                bossHPText.text = "0 / 0";
            }

            return;
        }

        currentHealth = Mathf.Clamp(
            currentHealth,
            0f,
            maximumHealth
        );

        bossHealthSlider.value =
            currentHealth / maximumHealth;

        if (bossHPText != null)
        {
            bossHPText.text =
                $"{Mathf.CeilToInt(currentHealth)} / " +
                $"{Mathf.CeilToInt(maximumHealth)}";
        }
    }

    public void SetBossHealthNormalized(float normalizedHealth)
    {
        if (bossHealthSlider == null)
        {
            return;
        }

        bossHealthSlider.value =
            Mathf.Clamp01(normalizedHealth);
    }

    public void HideBoss()
    {
        SetBossVisible(false);
    }

    private void SetBossVisible(bool visible)
    {
        if (bossCanvasGroup == null)
        {
            return;
        }

        bossCanvasGroup.alpha = visible ? 1f : 0f;
        bossCanvasGroup.interactable = false;
        bossCanvasGroup.blocksRaycasts = false;
    }
}