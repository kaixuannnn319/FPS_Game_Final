using UnityEngine;

// Put this on the SAME GameObject as the Animator (the boss model).
// Animation Events can only call methods on the object the Animator lives on,
// not on children — this forwards those calls to the actual weapon hitboxes.
//
// Each slot accepts either a single WeaponHitbox (simple weapon, one collider
// or several on the SAME object) or a WeaponHitboxGroup (rigged weapon like a
// whip, with hitboxes spread across multiple bone segments) — both expose
// the same EnableHitbox()/DisableHitbox() methods, so either works here.
public class BossAttackEvents : MonoBehaviour
{
    private Boss boss;

    private void Awake()
    {
        boss = GetComponentInParent<Boss>();
    }

    // Call from an Animation Event at the release frame of the shotgun/ranged attack clip
    public void FireShotgun()
    {
        if (boss != null) boss.FireShotgun();
    }

    [Tooltip("Drag the FIRST weapon's WeaponHitbox (simple weapon) or WeaponHitboxGroup (rigged weapon) here")]
    public MonoBehaviour weaponHitboxA;

    [Tooltip("Drag the SECOND weapon's WeaponHitbox or WeaponHitboxGroup here")]
    public MonoBehaviour weaponHitboxB;

    // ---- Weapon A ----
    public void EnableHitboxA() => InvokeEnable(weaponHitboxA);
    public void DisableHitboxA() => InvokeDisable(weaponHitboxA);

    // ---- Weapon B ----
    public void EnableHitboxB() => InvokeEnable(weaponHitboxB);
    public void DisableHitboxB() => InvokeDisable(weaponHitboxB);

    // ---- Both at once ----
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

    private void InvokeEnable(MonoBehaviour hitbox)
    {
        if (hitbox is WeaponHitbox wh) wh.EnableHitbox();
        else if (hitbox is WeaponHitboxGroup whg) whg.EnableHitbox();
    }

    private void InvokeDisable(MonoBehaviour hitbox)
    {
        if (hitbox is WeaponHitbox wh) wh.DisableHitbox();
        else if (hitbox is WeaponHitboxGroup whg) whg.DisableHitbox();
    }
}