using UnityEngine;

// Put this on the weapon mesh/bone itself (e.g. the sword blade child object).
// Needs a Collider with "Is Trigger" checked. Starts disabled — only "live"
// during the swing frames, toggled via BossAttackEvents.
[RequireComponent(typeof(Collider))]
public class WeaponHitbox : MonoBehaviour
{
    public float damage = 20f;

    private Collider hitCollider;
    private bool alreadyHitThisSwing; // prevents multiple hits from one swing if player stays inside

    private void Awake()
    {
        hitCollider = GetComponent<Collider>();
        hitCollider.isTrigger = true;
        hitCollider.enabled = false; // off by default
    }

    public void EnableHitbox()
    {
        alreadyHitThisSwing = false;
        hitCollider.enabled = true;
    }

    public void DisableHitbox()
    {
        hitCollider.enabled = false;
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