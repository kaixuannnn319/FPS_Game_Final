using UnityEngine;

// Patrols waypoints, chases player when spotted, attacks in melee range.
public class MeleeGuard : EnemyBase
{
    [Header("Patrol")]
    public Transform[] patrolPoints;
    public float patrolWaitTime = 2f;
    private int patrolIndex;
    private float waitTimer;

    [Header("Taunt")]
    public float tauntDuration = 1.2f; // matches the length of your taunt animation clip
    private float tauntTimer;

    [Header("Combat")]
    public float[] attackRanges = new float[] { 2f }; // one range per variant — index must match AttackIndex
    public int attackVariantCount = 1; // set to how many attack animations you've wired up (e.g. 3)
    public float[] attackCooldowns = new float[] { 1.5f }; // one cooldown per variant — index must match AttackIndex
    public float[] attackAnimationLockTimes = new float[] { 2.5f }; // one lock time per variant — total seconds that clip+rest sequence takes
    public float closeGapTimeout = 3f; // how long it'll keep trying to close in for a short-range attack before settling for a long-range one
    public float[] attackLaunchDistances = new float[] { 0f }; // meters to launch forward during this attack — 0 = no launch, one per variant
    public float attackLaunchDuration = 0.25f; // how many seconds the launch takes to complete
    public bool[] attackFreeRotation = new bool[] { false }; // true = keep tracking player during this attack's swing, false = lock facing at swing start
    private float lockTimer;
    protected int lastAttackIndex;
    private float closeGapTimer;
    private float launchTimer;
    private Vector3 launchVelocity;

    // Largest range across all variants — used to decide when to even enter Attack state at all
    protected float MaxAttackRange()
    {
        float max = 0f;
        if (attackRanges == null || attackRanges.Length == 0) return 2f; // fallback
        foreach (float r in attackRanges) if (r > max) max = r;
        return max;
    }

    // Smallest range across all variants — the "preferred" attack, worth closing the gap for
    private float ShortestAttackRange()
    {
        if (attackRanges == null || attackRanges.Length == 0) return 2f; // fallback
        float min = attackRanges[0];
        foreach (float r in attackRanges) if (r < min) min = r;
        return min;
    }

    // Picks randomly among whichever attack(s) share the SHORTEST range that can still
    // reach the player — favors close-range attacks over long-range ones, and randomizes
    // fairly if multiple attacks share that same shortest range.
    private int PickAttackIndexInRange()
    {
        float dist = DistanceToPlayer();
        float bestRange = float.MaxValue;
        var candidates = new System.Collections.Generic.List<int>();

        for (int i = 0; i < attackVariantCount && i < attackRanges.Length; i++)
        {
            if (dist > attackRanges[i]) continue; // can't reach with this one

            if (attackRanges[i] < bestRange)
            {
                bestRange = attackRanges[i];
                candidates.Clear();
                candidates.Add(i);
            }
            else if (Mathf.Approximately(attackRanges[i], bestRange))
            {
                candidates.Add(i);
            }
        }

        if (candidates.Count == 0) return 0; // fallback — shouldn't normally happen
        return candidates[Random.Range(0, candidates.Count)];
    }
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
                if (CanSeePlayer())
                {
                    if (!hasFiredDetectedEvent)
                    {
                        hasFiredDetectedEvent = true;
                        OnPlayerDetected?.Invoke();
                        OnHealthChanged?.Invoke(currentHealth, maxHealth); // send initial value so the bar starts correctly filled
                    }
                    agent.isStopped = true;
                    anim.SetTrigger(TauntParam);
                    tauntTimer = tauntDuration;
                    currentState = State.Taunt;
                }
                break;

            case State.Taunt:
                tauntTimer -= Time.deltaTime;
                if (tauntTimer <= 0f)
                {
                    agent.isStopped = false;
                    currentState = State.Chase;
                }
                break;

            case State.Chase:
                agent.SetDestination(GetChaseDestination());

                if (DistanceToPlayer() <= ShortestAttackRange())
                {
                    closeGapTimer = 0f;
                    SnapFacePlayer(); // lock facing direction once, right as the attack starts
                    currentState = State.Attack;
                }
                else if (DistanceToPlayer() <= MaxAttackRange())
                {
                    // Close enough for a long-range attack, but keep trying to close in for the short one first
                    closeGapTimer += Time.deltaTime;
                    if (closeGapTimer >= closeGapTimeout)
                    {
                        closeGapTimer = 0f;
                        SnapFacePlayer(); // lock facing direction once, right as the attack starts
                        currentState = State.Attack;
                    }
                }
                else
                {
                    closeGapTimer = 0f;
                    if (!CanSeePlayer())
                    {
                        hasFiredDetectedEvent = false; // allow re-detection to fire the event again later
                        OnPlayerLost?.Invoke();
                        currentState = State.Patrol; // lost the player, go back to patrolling
                    }
                }
                break;

            case State.Attack:
                agent.isStopped = true; // fully stop, not just destination = self
                lockTimer -= Time.deltaTime;

                if (attackFreeRotation != null && lastAttackIndex < attackFreeRotation.Length && attackFreeRotation[lastAttackIndex])
                    FacePlayer(); // this attack allows continued tracking during the swing, instead of a locked facing

                if (launchTimer > 0f)
                {
                    transform.position += launchVelocity * Time.deltaTime;
                    launchTimer -= Time.deltaTime;
                }

                if (lockTimer <= 0f)
                {
                    // Sequence just finished — snap back onto the NavMesh in case root motion pushed us off it
                    agent.updatePosition = true;

                    if (UnityEngine.AI.NavMesh.SamplePosition(transform.position, out UnityEngine.AI.NavMeshHit hit, 2f, UnityEngine.AI.NavMesh.AllAreas))
                    {
                        transform.position = hit.position;
                        agent.Warp(hit.position);
                    }
                    else
                    {
                        agent.Warp(transform.position); // fallback, shouldn't normally hit this
                    }

                    if (DistanceToPlayer() > MaxAttackRange())
                    {
                        agent.isStopped = false;
                        closeGapTimer = 0f;
                        currentState = State.Chase;
                    }
                    else if (DistanceToPlayer() > ShortestAttackRange())
                    {
                        // In range for a ranged attack, but melee is still out of reach — don't just
                        // keep re-firing ranged forever. Count how long that's been true, and once it
                        // passes the timeout, go back to Chase to try closing the gap again.
                        closeGapTimer += Time.deltaTime;
                        if (closeGapTimer >= closeGapTimeout)
                        {
                            closeGapTimer = 0f;
                            agent.isStopped = false;
                            currentState = State.Chase;
                        }
                        else if (attackTimer <= 0f)
                        {
                            FireAttack();
                            closeGapTimer = closeGapTimeout; // only fire once — next time the lock expires, go straight back to Chase
                        }
                        else
                        {
                            FacePlayer();
                        }
                    }
                    else
                    {
                        // Melee is reachable — normal flow, no gap-closing concerns
                        closeGapTimer = 0f;
                        if (attackTimer <= 0f)
                            FireAttack();
                        else
                            FacePlayer();
                    }
                }
                break;
        }
    }

    [Header("Arena Bounds")]
    [Tooltip("Size of the allowed area (X = width, Z = depth), centered on its spawn point. Set X or Z to 0 for no limit on that axis.")]
    public Vector2 arenaSize = new Vector2(30f, 30f);

    // Clamps a destination so it never leaves the arena box — measured from spawnPosition, not the
    // player, so the boss can't get pulled/pushed outside its intended area no matter how far the player runs.
    protected Vector3 ClampToArena(Vector3 destination)
    {
        float halfX = arenaSize.x / 2f;
        float halfZ = arenaSize.y / 2f; // Vector2.y represents depth (Z) here

        if (arenaSize.x > 0f)
            destination.x = Mathf.Clamp(destination.x, spawnPosition.x - halfX, spawnPosition.x + halfX);

        if (arenaSize.y > 0f)
            destination.z = Mathf.Clamp(destination.z, spawnPosition.z - halfZ, spawnPosition.z + halfZ);

        return destination;
    }

    // Where to walk toward while chasing. Default: straight to the player, clamped to the arena.
    // Override in subclasses (e.g. a ranged/caster boss) to stop short and keep distance instead.
    protected virtual Vector3 GetChaseDestination()
    {
        return ClampToArena(player.position);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (arenaSize.x <= 0f && arenaSize.y <= 0f) return;
        Gizmos.color = Color.cyan;
        Vector3 center = Application.isPlaying ? spawnPosition : transform.position;
        Vector3 size = new Vector3(
            arenaSize.x > 0f ? arenaSize.x : 200f,
            0.1f,
            arenaSize.y > 0f ? arenaSize.y : 200f
        );
        Gizmos.DrawWireCube(center, size);
    }

    // Actually fires the currently-chosen attack — picks a variant, triggers the animation,
    // sets its cooldown/lock time, and kicks off any launch. Shared by both the
    // "melee reachable" and "ranged-only" paths in the Attack state.
    private void FireAttack()
    {
        agent.updatePosition = false; // hand control to root motion again for the next swing
        SnapFacePlayer(); // lock facing direction once, right as this swing starts
        lastAttackIndex = PickAttackIndexInRange();
        anim.SetInteger(AttackIndexParam, lastAttackIndex);
        anim.SetTrigger(AttackParam);
        StartLaunch(lastAttackIndex);

        float cooldown = (attackCooldowns != null && lastAttackIndex < attackCooldowns.Length)
            ? attackCooldowns[lastAttackIndex]
            : 1.5f; // fallback if array wasn't sized correctly
        attackTimer = cooldown;

        lockTimer = (attackAnimationLockTimes != null && lastAttackIndex < attackAnimationLockTimes.Length)
            ? attackAnimationLockTimes[lastAttackIndex]
            : 2.5f; // fallback if array wasn't sized correctly
        // Hook actual damage application to an Animation Event on the attack clip
        // (call DealDamage() at the moment the weapon hits, not here directly)
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

    // Kicks off a manual forward dash for attack variants that have a launch distance set.
    // Independent of root motion — works even if the clip itself has no baked movement.
    private void StartLaunch(int attackIndex)
    {
        float distance = (attackLaunchDistances != null && attackIndex < attackLaunchDistances.Length)
            ? attackLaunchDistances[attackIndex]
            : 0f;

        if (distance > 0f)
        {
            launchVelocity = transform.forward * (distance / attackLaunchDuration);
            launchTimer = attackLaunchDuration;
        }
        else
        {
            launchTimer = 0f;
        }
    }

    private void FacePlayer()
    {
        Vector3 dir = (player.position - transform.position);
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10f * Time.deltaTime);
    }

    // Instantly snaps to face the player, no gradual turning — used right as a swing starts
    // so the attack always fires in the correct direction, regardless of how far off it was before.
    private void SnapFacePlayer()
    {
        Vector3 dir = (player.position - transform.position);
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    // Call this from an Animation Event placed on the attack clip's "hit" frame
    public virtual void DealDamage()
    {
        float range = (attackRanges != null && lastAttackIndex < attackRanges.Length)
            ? attackRanges[lastAttackIndex]
            : MaxAttackRange();

        if (DistanceToPlayer() <= range + 0.5f)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage((int)attackDamage);
        }
    }
}