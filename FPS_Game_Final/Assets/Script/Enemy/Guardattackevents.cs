using UnityEngine;

// Put this on the SAME GameObject as the Animator (the model, the child).
// Animation Events can only call methods on the object the Animator lives on —
// this just forwards the call up to MeleeGuard/Boss on the parent.
public class GuardAttackEvents : MonoBehaviour
{
    private MeleeGuard guard;

    private void Awake()
    {
        guard = GetComponentInParent<MeleeGuard>();
    }

    // Call from an Animation Event at the hit frame of the attack clip
    public void DealDamage()
    {
        if (guard != null) guard.DealDamage();
    }
}