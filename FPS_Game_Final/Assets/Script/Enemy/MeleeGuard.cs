using UnityEngine;

// Patrols waypoints, chases player when spotted, attacks in melee range.
public class MeleeGuard : EnemyBase
{
    [Header("Patrol")]
    public Transform[] patrolPoints;
    public float patrolWaitTime = 2f;
    private int patrolIndex;
    private float waitTimer;

    [Header("Combat")]
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public float attackDamage = 15f;
    private float attackTimer;

    protected override void Awake()
    {
        base.Awake();
        if (patrolPoints.Length > 0)
            agent.SetDestination(patrolPoints[0].position);
    }

    protected override void Update()
    {
        base.Update();
        if (currentState == State.Dead) return;

        attackTimer -= Time.deltaTime;

        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                if (CanSeePlayer()) currentState = State.Chase;
                break;

            case State.Chase:
                agent.SetDestination(player.position);
                if (DistanceToPlayer() <= attackRange)
                    currentState = State.Attack;
                else if (!CanSeePlayer())
                    currentState = State.Patrol; // lost the player, go back to patrolling
                break;

            case State.Attack:
                agent.SetDestination(transform.position); // stand still to attack
                FacePlayer();
                if (DistanceToPlayer() > attackRange)
                {
                    currentState = State.Chase;
                }
                else if (attackTimer <= 0f)
                {
                    anim.SetTrigger(AttackParam);
                    attackTimer = attackCooldown;
                    // Hook actual damage application to an Animation Event on the attack clip
                    // (call DealDamage() at the moment the weapon hits, not here directly)
                }
                break;
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= patrolWaitTime)
            {
                patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
                agent.SetDestination(patrolPoints[patrolIndex].position);
                waitTimer = 0f;
            }
        }
    }

    private void FacePlayer()
    {
        Vector3 dir = (player.position - transform.position);
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10f * Time.deltaTime);
    }

    // Call this from an Animation Event placed on the attack clip's "hit" frame
    public void DealDamage()
    {
        if (DistanceToPlayer() <= attackRange + 0.5f)
        {
            // Replace with your actual player health/damage interface
            player.GetComponent<IDamageable>()?.TakeDamage(attackDamage);
        }
    }
}