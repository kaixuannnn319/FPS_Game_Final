using UnityEngine;

// Put this on the weapon object. Supports MULTIPLE colliders on the same object
// (e.g. several boxes along a long blade for better hit coverage) — all of them
// get enabled/disabled together, and a hit from ANY of them counts as one hit.
// Each collider needs "Is Trigger" checked. Starts disabled — only "live"
// during the swing frames, toggled via BossAttackEvents/GuardAttackEvents.
public class WeaponHitbox : MonoBehaviour
{
    public float damage = 20f;

    private Collider[] hitColliders;
    private bool alreadyHitThisSwing; // prevents multiple hits from one swing, even across multiple colliders

    private void Awake()
    {
        hitColliders = GetComponents<Collider>();

        if (hitColliders.Length == 0)
            Debug.LogWarning($"WeaponHitbox on '{gameObject.name}' has no Collider attached — add at least one and check Is Trigger.");

        foreach (var col in hitColliders)
        {
            col.isTrigger = true;
            col.enabled = false; // off by default
        }
    }

    public void EnableHitbox()
    {
        alreadyHitThisSwing = false;
        foreach (var col in hitColliders) col.enabled = true;
    }

    public void DisableHitbox()
    {
        foreach (var col in hitColliders) col.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyHitThisSwing) return;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage((int)damage);
            alreadyHitThisSwing = true;
        }
    }
}