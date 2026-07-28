using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

// Shared logic for all enemy types: health, death, player detection, animator sync.
[RequireComponent(typeof(NavMeshAgent))]
public abstract class EnemyBase : MonoBehaviour
{
    public enum State { Patrol, Taunt, Chase, Attack, Dead }

    [Header("Stats")]
    public float maxHealth = 100f;
    public float detectionRange = 12f;

    [Header("Testing")]
    [Tooltip("If ON, enemy won't be Destroyed on death, so it can be revived with Respawn(). Turn OFF for real gameplay.")]
    public bool testMode = false;
    protected Vector3 spawnPosition;
    protected Quaternion spawnRotation;

    [Header("Item Drop")]
    [Tooltip("Prefab to spawn on death. Leave empty for no drop. Your teammate's pickup/interaction script should live on this prefab.")]
    public GameObject itemDropPrefab;
    [Range(0f, 1f)] public float dropChance = 1f; // 1 = always drops, 0.3 = 30% chance, etc.

    [Header("Boss Progress")]
    [SerializeField] private bool isBoss = false;
    [SerializeField] private BossID bossID = BossID.None;

    protected float currentHealth;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    [Header("UI Events")]
    [Tooltip("Fires whenever health changes — (currentHealth, maxHealth). Wire this to BossHealthUIController.SetBossHealth in the Inspector.")]
    public UnityEvent<float, float> OnHealthChanged;
    [Tooltip("Fires once, the moment this enemy first detects the player — good for showing a boss health bar. Wire to BossHealthUIController.ShowBoss (needs a name, so use a wrapper or SetBossVisible-equivalent) or just SetBossVisible via a small adapter.")]
    public UnityEvent OnPlayerDetected;
    [Tooltip("Fires when this enemy dies — wire to BossHealthUIController.HideBoss.")]
    public UnityEvent OnEnemyDeath;
    protected bool hasFiredDetectedEvent;
    protected State currentState = State.Patrol;
    protected Transform player;
    protected NavMeshAgent agent;
    [SerializeField] protected Animator anim; // drag the CHILD model's Animator here manually

    // Animator parameter names — must match what you set up in the Animator Controller
    protected static readonly int SpeedParam = Animator.StringToHash("Speed");
    protected static readonly int AttackParam = Animator.StringToHash("Attack");
    protected static readonly int DieParam = Animator.StringToHash("Die");
    protected static readonly int TauntParam = Animator.StringToHash("Taunt");
    protected static readonly int AttackIndexParam = Animator.StringToHash("AttackIndex");

    protected virtual void OnEnable()
    {
        GameEvents.OnPlayerRespawn += HandlePlayerRespawn;
    }

    protected virtual void OnDisable()
    {
        GameEvents.OnPlayerRespawn -= HandlePlayerRespawn;
    }

    private void HandlePlayerRespawn()
    {
        // Only revive enemies that are dead — don't reset ones still alive/mid-fight.
        if (currentState == State.Dead)
            Respawn();
    }

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (anim == null) anim = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
        Debug.Log(gameObject.name + " found player: " + (player != null ? player.name : "NULL")); // TEMP — remove once working

        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
    }

    protected virtual void Update()
    {
        if (currentState == State.Dead) return;

        // Drive the Blend Tree / Idle-Walk transition off agent speed
        anim.SetFloat(SpeedParam, agent.velocity.magnitude);
    }

    protected float DistanceToPlayer()
    {
        if (player == null) return Mathf.Infinity;
        return Vector3.Distance(transform.position, player.position);
    }

    protected bool CanSeePlayer()
    {
        return DistanceToPlayer() <= detectionRange;
    }

    public virtual void TakeDamage(float amount)
    {
        if (currentState == State.Dead) return;

        currentHealth -= amount;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0f)
            Die();
    }

    protected virtual void Die()
    {
        currentState = State.Dead;
        agent.isStopped = true;
        anim.SetTrigger(DieParam);
        OnEnemyDeath?.Invoke();

        if (isBoss)
        {
            GameManager.Instance.OnBossDefeated(bossID);
        }

        // Disable colliders so it doesn't block the player/bullets after dying
        foreach (var col in GetComponents<Collider>())
            col.enabled = false;

        if (itemDropPrefab != null && Random.value <= dropChance)
            Instantiate(itemDropPrefab, transform.position, Quaternion.identity);

        if (!testMode)
            Destroy(gameObject, 5f); // give the death animation time to play
        // In test mode, the object stays in the scene (just dead) so Respawn() can revive it.
    }

    // TEST ONLY — call from a debug key/button to revive the enemy at its original spawn point.
    public virtual void Respawn()
    {
        currentHealth = maxHealth;
        currentState = State.Patrol;

        transform.position = spawnPosition;
        transform.rotation = spawnRotation;

        agent.Warp(spawnPosition); // safely repositions a NavMeshAgent (avoids desync issues)
        agent.isStopped = false;

        foreach (var col in GetComponents<Collider>())
            col.enabled = true;

        anim.Rebind();  // resets Animator out of the Death state
        anim.Update(0f);
    }
}