using UnityEngine;
using UnityEngine.SceneManagement;

public enum DeathType
{
    Enemy,
    Water
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private string level1Scene = "Level 1";
    [SerializeField] private string level2Scene = "Level 2";
    [SerializeField] private string bossScene = "Bxx_Map";

    [Header("Respawn")]
    [SerializeField] private float respawnDelay = 1f;
    [SerializeField] private Transform defaultSpawnPoint;

    public bool level1BossDead;
    public int level2BossesDefeated;
    public bool finalBossDead;

    private DeathType lastDeathType;

    private Transform currentCheckpoint;
    private GameObject player;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadLevel1()
    {
        SceneManager.LoadScene(level1Scene);
    }

    public void LoadLevel2()
    {
        if (player != null)
        {
            InventoryController inventory =
                player.GetComponent<InventoryController>();

            if (inventory != null)
                inventory.SaveInventory();
        }

        SceneManager.LoadScene(level2Scene);
    }
    public void RegisterPlayer(GameObject newPlayer)
    {
        player = newPlayer;

        Debug.Log("Player Registered");
    }

    public void LoadBossMap()
    {
        if (player != null)
        {
            InventoryController inventory =
                player.GetComponent<InventoryController>();

            if (inventory != null)
                inventory.SaveInventory();
        }

        SceneManager.LoadScene(bossScene);
    }

    public void RegisterCheckpoint(Transform checkpoint)
    {
        currentCheckpoint = checkpoint;

        Debug.Log("Checkpoint Updated : " + checkpoint.name);
    }
    public void PlayerDied(DeathType deathType)
    {
        lastDeathType = deathType;

        Debug.Log("Player Died : " + deathType);

        Invoke(nameof(RespawnPlayer), respawnDelay);
    }
    private void RespawnPlayer()
    {
        if (player == null)
        {
            Debug.LogError("Player is Missing!");
            return;
        }

        Transform spawnPoint = currentCheckpoint != null
            ? currentCheckpoint
            : defaultSpawnPoint;

        if (spawnPoint == null)
        {
            Debug.LogError("No Spawn Point Assigned!");
            return;
        }

        // Move player
        player.transform.position = spawnPoint.position;
        player.transform.rotation = spawnPoint.rotation;

        // Restore checkpoint inventory after enemy death
        if (lastDeathType == DeathType.Enemy)
        {
            InventoryController inventory =
                player.GetComponent<InventoryController>();

            if (inventory != null)
            {
                inventory.RestoreCheckpointInventory();
            }

            if (PickupManager.Instance != null)
            {
                PickupManager.Instance.ResetPickups();
            }
        }

        // Restore HP
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.ResetHealth();
        }

        // Enable movement
        PlayerController controller = player.GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.enabled = true;
        }

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Player Respawned");
    }
    public void SetDefaultSpawnPoint(Transform spawn)
    {
        defaultSpawnPoint = spawn;
        currentCheckpoint = spawn;

        if (currentCheckpoint == null)
        {
            currentCheckpoint = spawn;
        }
    }

    public void OnBossDefeated(BossID boss)
    {
        switch (boss)
        {
            case BossID.Level1Boss:
                level1BossDead = true;
                break;

            case BossID.Level2BossA:
            case BossID.Level2BossB:
                level2BossesDefeated++;
                break;

            case BossID.FinalBoss:
                finalBossDead = true;
                break;
        }
    }
}
