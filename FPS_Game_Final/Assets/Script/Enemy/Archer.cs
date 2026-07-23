using UnityEngine;
using UnityEngine.ProBuilder;

// Stays in place, faces and shoots the player when in range. No patrol/chase.
public class Archer : EnemyBase
{
    [Header("Combat")]
    public float attackRange = 15f;
    public float attackCooldown = 2f;
    public float projectileSpeed = 20f;
    public float attackDamage = 10f;
    public Transform shootPoint;      // empty GameObject at the bow/hand
    public GameObject projectilePrefab;
    private float attackTimer;

    protected override void Awake()
    {
        base.Awake();
        agent.isStopped = true; // archer doesn't move
    }

    protected override void Update()
    {
        base.Update();
        if (currentState == State.Dead) return;

        attackTimer -= Time.deltaTime;

        switch (currentState)
        {
            case State.Patrol: // used here as "idle" — archer just waits
                if (CanSeePlayer() && DistanceToPlayer() <= attackRange)
                    currentState = State.Attack;
                break;

            case State.Attack:
                FacePlayer();
                if (DistanceToPlayer() > attackRange)
                {
                    currentState = State.Patrol;
                }
                else if (attackTimer <= 0f)
                {
                    anim.SetTrigger(AttackParam);
                    attackTimer = attackCooldown;
                    // Actual arrow spawn happens in FireArrow(), called via Animation Event
                }
                break;
        }
    }

    private void FacePlayer()
    {
        Vector3 dir = (player.position - transform.position);
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10f * Time.deltaTime);
    }

    // Call this from an Animation Event on the "release" frame of the shoot clip
    public void FireArrow()
    {
        if (projectilePrefab == null || shootPoint == null || player == null) return;

        GameObject arrow = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        Vector3 dir = (player.position + Vector3.up - shootPoint.position).normalized;

        if (arrow.TryGetComponent(out Rigidbody rb))
            rb.linearVelocity = dir * projectileSpeed; // use rb.velocity if on Unity < 6

        if (arrow.TryGetComponent(out Projectile proj))
            proj.damage = attackDamage;
    }
}