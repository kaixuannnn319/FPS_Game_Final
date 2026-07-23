using UnityEngine;

// Same patrol/chase/attack loop as MeleeGuard, plus a phase-2 stat boost at half health.
public class Boss : MeleeGuard
{
    [Header("Boss Phase 2")]
    public float phase2HealthThreshold = 0.5f; // trigger at 50% HP
    public float phase2SpeedMultiplier = 1.4f;
    public float phase2DamageMultiplier = 1.5f;
    private bool phase2Triggered = false;

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        if (!phase2Triggered && currentHealth <= maxHealth * phase2HealthThreshold)
        {
            phase2Triggered = true;
            EnterPhase2();
        }
    }

    private void EnterPhase2()
    {
        agent.speed *= phase2SpeedMultiplier;
        attackDamage *= phase2DamageMultiplier;
        attackCooldown *= 0.7f; // attacks faster too
        // Optional: anim.SetTrigger("Enrage") if your model has a rage/roar animation
    }
}