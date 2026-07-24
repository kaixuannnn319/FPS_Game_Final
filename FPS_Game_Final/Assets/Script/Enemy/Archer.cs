using UnityEngine;

// Stationary at its spawn point by default. While the player is within
// detectionRange, it's allowed to move (e.g. reposition to keep attackRange).
// Once the player leaves detectionRange, it walks back to its spawn point
// and locks in place again.
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

    [Header("Return To Spawn")]
    public float returnDistanceThreshold = 0.3f; // how close to spawn counts as "arrived"

    protected override void Awake()
    {
        base.Awake();
        agent.isStopped = true; // stationary until it detects the player
        agent.updateRotation = false; // script handles all rotation manually, avoids fighting with the agent
    }

    protected override void Update()
    {
        base.Update();
        if (currentState == State.Dead) return;

        attackTimer -= Time.deltaTime;

        switch (currentState)
        {
            case State.Patrol: // used here as "idle at spawn"
                if (CanSeePlayer())
                {
                    agent.isStopped = false; // allowed to move now
                    currentState = DistanceToPlayer() <= attackRange ? State.Attack : State.Chase;
                }
                break;

            case State.Chase:
                if (!CanSeePlayer())
                {
                    ReturnToSpawn();
                    break;
                }
                agent.SetDestination(player.position);
                FaceMoveDirection();
                if (DistanceToPlayer() <= attackRange)
                    currentState = State.Attack;
                break;

            case State.Attack:
                if (!CanSeePlayer())
                {
                    ReturnToSpawn();
                    break;
                }
                agent.SetDestination(transform.position); // hold position to shoot
                FacePlayer();
                if (DistanceToPlayer() > attackRange)
                {
                    currentState = State.Chase;
                }
                else if (attackTimer <= 0f)
                {
                    anim.SetTrigger(AttackParam);
                    attackTimer = attackCooldown;
                }
                break;

            case State.Taunt: // reused here as "returning to spawn"
                FaceMoveDirection();
                if (Vector3.Distance(transform.position, spawnPosition) <= returnDistanceThreshold)
                {
                    agent.isStopped = true;
                    transform.rotation = spawnRotation;
                    currentState = State.Patrol;
                }
                else if (CanSeePlayer())
                {
                    // player came back into range before it finished returning — re-engage
                    currentState = DistanceToPlayer() <= attackRange ? State.Attack : State.Chase;
                }
                break;
        }
    }

    private void ReturnToSpawn()
    {
        agent.SetDestination(spawnPosition);
        currentState = State.Taunt; // reused as "returning" state, see above
    }

    private void FacePlayer()
    {
        Vector3 dir = (player.position - transform.position);
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10f * Time.deltaTime);
    }

    // Faces wherever the agent is currently moving — used for Chase/Return so it
    // doesn't fight with the agent's own rotation (Update Rotation should be OFF).
    private void FaceMoveDirection()
    {
        Vector3 dir = agent.velocity;
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