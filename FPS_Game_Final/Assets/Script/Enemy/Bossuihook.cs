using UnityEngine;

// Put this on the boss GameObject (same one with Boss/FloatingBoss/etc).
// Handles connecting this specific boss's name to the shared health bar UI —
// the other events (health changed, death) can be wired directly in the
// Inspector since their parameters match SetBossHealth/HideBoss exactly.
public class BossUIHook : MonoBehaviour
{
    public string bossDisplayName = "BOSS";
    public BossHealthUIController bossUI;
    private EnemyBase enemy;

    private void Awake()
    {
        enemy = GetComponent<EnemyBase>();
    }

    // Wire this to OnPlayerDetected in the Inspector
    public void ShowThisBoss()
    {
        if (bossUI != null && enemy != null)
            bossUI.ShowBoss(bossDisplayName, enemy.CurrentHealth, enemy.MaxHealth);
    }
}