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

        for (int i = 0; i < attackCooldowns.Length; i++)
            attackCooldowns[i] *= 0.7f; // attacks faster too

        // Optional: anim.SetTrigger("Enrage") if your model has a rage/roar animation
    }

    // Boss uses a weapon hitbox (WeaponHitbox.cs) instead of the distance-check
    // damage MeleeGuard uses — override DealDamage to do nothing so it can't double-hit.
    public override void DealDamage() { }

    [Header("Shotgun Ranged Attack")]
    [Tooltip("Which entry in Attack Ranges/Cooldowns/etc. is this ranged attack")]
    public int rangedAttackIndex = 2;
    public GameObject pelletPrefab;
    public Transform castPoint; // empty GameObject at the muzzle/hand
    public int pelletCount = 6;
    public float spreadAngle = 20f; // total cone angle in degrees
    public float pelletDamage = 8f;
    public float pelletSpeed = 25f;

    // Call this from an Animation Event on the ranged attack clip, at the release frame.
    // Guarded against currentState so it won't fire if this same clip is also used for Taunt.
    public void FireShotgun()
    {
        if (currentState != State.Attack) return; // don't fire if this clip is playing as Taunt, not a real attack
        if (pelletPrefab == null || castPoint == null || player == null) return;

        Vector3 baseDir = (player.position + Vector3.up - castPoint.position).normalized;

        for (int i = 0; i < pelletCount; i++)
        {
            // Spread pellets evenly across the cone, centered on baseDir
            float t = pelletCount > 1 ? (float)i / (pelletCount - 1) : 0.5f; // 0..1 across the pellets
            float angle = Mathf.Lerp(-spreadAngle / 2f, spreadAngle / 2f, t);

            Quaternion spreadRot = Quaternion.AngleAxis(angle, Vector3.up); // horizontal spread
            Vector3 dir = spreadRot * baseDir;

            GameObject pellet = Instantiate(pelletPrefab, castPoint.position, Quaternion.LookRotation(dir));

            if (pellet.TryGetComponent(out Rigidbody rb))
                rb.linearVelocity = dir * pelletSpeed; // use rb.velocity if on Unity < 6

            if (pellet.TryGetComponent(out Projectile proj))
                proj.damage = pelletDamage;
        }
    }
}