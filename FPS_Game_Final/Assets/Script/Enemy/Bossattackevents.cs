using UnityEngine;

// Put this on the SAME GameObject as the Animator (the boss model).
// Animation Events can only call methods on the object the Animator lives on,
// not on children — this just forwards those calls to the actual weapon hitbox.
public class BossAttackEvents : MonoBehaviour
{
    [Tooltip("Drag the weapon's WeaponHitbox component here (child object with the collider)")]
    public WeaponHitbox weaponHitbox;

    // Call from an Animation Event at the frame the swing starts
    public void EnableHitbox()
    {
        if (weaponHitbox != null) weaponHitbox.EnableHitbox();
    }

    // Call from an Animation Event at the frame the swing ends
    public void DisableHitbox()
    {
        if (weaponHitbox != null) weaponHitbox.DisableHitbox();
    }
}