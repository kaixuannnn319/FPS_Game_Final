using UnityEngine;

// A boss that floats above the ground while still following the NavMesh, and can
// both cast ranged magic AND fight in melee. Reuses all of MeleeGuard's multi-attack
// system (patrol, taunt, chase, close-gap logic, per-variant range/cooldown/lock-time) —
// the ranged spell is just treated as one more attack variant with a large range.
//
// SETUP: set Attack Variant Count to include your melee attacks PLUS the spell attack.
// Example: 0 = melee swing (short range), 1 = melee swing 2 (short range),
//          2 = magic bolt (long range) — set attackRanges[2] big, e.g. 12.
// Assign "Ranged Attack Index" below to whichever index is the spell (2, in this example).
public class FloatingBoss : MeleeGuard
{
    [Header("Floating")]
    public float floatHeight = 1.5f; // how high above the NavMesh surface it hovers normally
    public float meleeSwoopHeight = 0.2f; // how low it dips down to actually land a melee hit
    public float heightChangeSpeed = 3f; // how fast it transitions between float/swoop height

    [Header("Ranged Magic Attack")]
    [Tooltip("Which entry in Attack Ranges / Attack Cooldowns / etc. is the magic spell (not melee)")]
    public int rangedAttackIndex = 2;
    public GameObject spellProjectilePrefab;
    public Transform castPoint; // empty GameObject at the hand/staff tip
    public float spellDamage = 20f;
    public float spellSpeed = 15f;

    [Header("Standoff")]
    [Tooltip("Preferred distance to keep from the player while chasing — won't walk closer than this on its own")]
    public float preferredDistance = 8f;

    // Instead of walking all the way to the player, stop short at preferredDistance —
    // still lets melee happen if the PLAYER walks in close, just won't close the gap itself.
    protected override Vector3 GetChaseDestination()
    {
        Vector3 dirFromPlayer = (transform.position - player.position);
        dirFromPlayer.y = 0f;

        if (dirFromPlayer.sqrMagnitude < 0.01f)
            dirFromPlayer = transform.forward; // fallback if standing exactly on top of the player

        dirFromPlayer.Normalize();
        return player.position + dirFromPlayer * preferredDistance;
    }

    protected override void Awake()
    {
        base.Awake();
        agent.baseOffset = floatHeight; // fixed hover height for the agent/collision — never changed after this
    }

    protected override void Update()
    {
        base.Update();
        if (currentState == State.Dead) return;

        // Dip down while actively mid-melee-attack (not the ranged spell), rise back up otherwise.
        // Applied to the PARENT's height directly, NOT the child model — the child's local position
        // gets forced back to zero every frame by RootMotionRelay, so any offset set there would be
        // wiped out immediately during root-motion-driven attacks. The parent's Y is safe to adjust
        // manually here since the agent doesn't touch it while updatePosition is false (during attacks).
        // Only touch height manually during attacks (when updatePosition is false and the agent
        // isn't writing position on its own). Outside of attacks, agent.baseOffset (fixed at
        // floatHeight in Awake) already handles the hover height automatically — don't fight it.
        if (currentState != State.Attack) return;

        bool doingMelee = lastAttackIndex != rangedAttackIndex;
        float targetOffset = doingMelee ? meleeSwoopHeight : floatHeight;

        Vector3 pos = transform.position;
        float currentOffset = pos.y - GroundYEstimate();
        float newOffset = Mathf.MoveTowards(currentOffset, targetOffset, heightChangeSpeed * Time.deltaTime);
        pos.y = GroundYEstimate() + newOffset;
        transform.position = pos;
    }

    // Rough estimate of the ground height directly below, so we can add our float/swoop offset on top of it
    // rather than an absolute world Y (works even if the arena isn't perfectly flat).
    private float GroundYEstimate()
    {
        if (UnityEngine.AI.NavMesh.SamplePosition(transform.position, out UnityEngine.AI.NavMeshHit hit, 5f, UnityEngine.AI.NavMesh.AllAreas))
            return hit.position.y;
        return transform.position.y - floatHeight; // fallback: assume current height is roughly correct
    }

    // Melee damage now comes from WeaponHitbox (collider-based) instead of this distance check —
    // override to do nothing so it can't double-hit if a leftover Animation Event still calls it.
    public override void DealDamage() { }

    // Call this from an Animation Event on the magic spell clip, at the moment the spell releases
    public void CastSpell()
    {
        if (spellProjectilePrefab == null || castPoint == null || player == null) return;

        Vector3 dir = (player.position + Vector3.up - castPoint.position).normalized;
        GameObject spell = Instantiate(spellProjectilePrefab, castPoint.position, Quaternion.LookRotation(dir));

        if (spell.TryGetComponent(out Rigidbody rb))
            rb.linearVelocity = dir * spellSpeed; // use rb.velocity if on Unity < 6

        if (spell.TryGetComponent(out Projectile proj))
            proj.damage = spellDamage;
    }
}