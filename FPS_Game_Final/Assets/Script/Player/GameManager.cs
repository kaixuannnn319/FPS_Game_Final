using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private string level1Scene = "Level 1";
    [SerializeField] private string level2Scene = "Level 2";
    [SerializeField] private string bossScene = "Bxx_Map";

    [Header("Respawn")]
    [SerializeField] private float respawnDelay = 2f;
    [SerializeField] private Transform defaultSpawnPoint;

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
        SceneManager.LoadScene(level2Scene);
    }
    public void RegisterPlayer(GameObject newPlayer)
    {
        player = newPlayer;

        Debug.Log("Player Registered");
    }

    public void LoadBossMap()
    {
        SceneManager.LoadScene(bossScene);
    }
    public void RegisterCheckpoint(Transform checkpoint)
    {
        currentCheckpoint = checkpoint;

        Debug.Log("Checkpoint Updated : " + checkpoint.name);
    }
    public void PlayerDied()
    {
        Debug.Log("Player Died");

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
}
