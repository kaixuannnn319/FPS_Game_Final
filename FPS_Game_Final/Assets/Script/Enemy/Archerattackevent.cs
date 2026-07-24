using UnityEngine;

// Put this on the SAME GameObject as the Animator (the archer model, the child).
// Animation Events can only call methods on the object the Animator lives on —
// this forwards the call up to Archer on the parent.
public class ArcherAttackEvents : MonoBehaviour
{
    private Archer archer;

    private void Awake()
    {
        archer = GetComponentInParent<Archer>();
    }

    // Call from an Animation Event at the release frame of the shoot clip
    public void FireArrow()
    {
        if (archer != null) archer.FireArrow();
    }
}