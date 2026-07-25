using UnityEngine;

// Put this on the SAME GameObject as the Animator (the boss model).
// Animation Events can only call methods on the object the Animator lives on,
// not on children — this forwards those calls to the actual weapon hitboxes.
public class BossAttackEvents : MonoBehaviour
{
    [Tooltip("Drag the FIRST weapon/hand's WeaponHitbox component here")]
    public WeaponHitbox weaponHitboxA;

    [Tooltip("Drag the SECOND weapon/hand's WeaponHitbox component here")]
    public WeaponHitbox weaponHitboxB;

    // ---- Weapon A ----
    public void EnableHitboxA()
    {
        if (weaponHitboxA != null) weaponHitboxA.EnableHitbox();
    }

    public void DisableHitboxA()
    {
        if (weaponHitboxA != null) weaponHitboxA.DisableHitbox();
    }

    // ---- Weapon B ----
    public void EnableHitboxB()
    {
        if (weaponHitboxB != null) weaponHitboxB.EnableHitbox();
    }

    public void DisableHitboxB()
    {
        if (weaponHitboxB != null) weaponHitboxB.DisableHitbox();
    }

    // ---- Both at once (useful for a dual-wield swing that hits with both simultaneously) ----
    public void EnableHitboxBoth()
    {
        EnableHitboxA();
        EnableHitboxB();
    }

    public void DisableHitboxBoth()
    {
        DisableHitboxA();
        DisableHitboxB();
    }
}