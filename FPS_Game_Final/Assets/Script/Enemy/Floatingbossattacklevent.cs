using UnityEngine;

// Put this on the SAME GameObject as the Animator (the floating boss's model).
// Animation Events can only call methods on the object the Animator lives on —
// this forwards those calls up to FloatingBoss (spell casting) and to the
// weapon hitbox(es) directly (melee damage), on the parent/children.
public class FloatingBossAttackEvents : MonoBehaviour
{
    private FloatingBoss boss;

    [Tooltip("Drag the melee hand/weapon's WeaponHitbox component here")]
    public WeaponHitbox weaponHitboxA;

    [Tooltip("Optional second hitbox (e.g. other hand), leave empty if not needed")]
    public WeaponHitbox weaponHitboxB;

    private void Awake()
    {
        boss = GetComponentInParent<FloatingBoss>();
    }

    // Call from an Animation Event at the release frame of the MAGIC spell clip
    public void CastSpell()
    {
        if (boss != null) boss.CastSpell();
    }

    // ---- Melee hitbox control — call from Animation Events on melee attack clips ----
    public void EnableHitboxA()
    {
        if (weaponHitboxA != null) weaponHitboxA.EnableHitbox();
    }

    public void DisableHitboxA()
    {
        if (weaponHitboxA != null) weaponHitboxA.DisableHitbox();
    }

    public void EnableHitboxB()
    {
        if (weaponHitboxB != null) weaponHitboxB.EnableHitbox();
    }

    public void DisableHitboxB()
    {
        if (weaponHitboxB != null) weaponHitboxB.DisableHitbox();
    }
}