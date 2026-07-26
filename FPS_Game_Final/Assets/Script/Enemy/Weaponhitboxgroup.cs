using UnityEngine;

// Put this on the ROOT of the whip (or any rigged weapon where hit colliders
// live on separate bone segments instead of one single object). Put a
// WeaponHitbox + Collider on each bone segment that should register hits,
// then drag all of them into the array below. This group can then be
// dropped into BossAttackEvents' Weapon Hitbox slots just like a single one.
public class WeaponHitboxGroup : MonoBehaviour
{
    [Tooltip("One WeaponHitbox per bone segment that should register hits")]
    public WeaponHitbox[] segments;

    public void EnableHitbox()
    {
        foreach (var seg in segments)
            if (seg != null) seg.EnableHitbox();
    }

    public void DisableHitbox()
    {
        foreach (var seg in segments)
            if (seg != null) seg.DisableHitbox();
    }
}