using TMPro;
using UnityEngine;
public class ClueDocumentUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject documentRoot;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text bodyText;
    [Header("Behaviour")]
    [SerializeField] private bool pauseGame = true;
    [SerializeField] private float closeInputDelay = 0.15f;
    [Header("Temporary Test")]
    [SerializeField] private bool enableTestKey = true;
    [SerializeField] private KeyCode testOpenKey = KeyCode.T;
    [SerializeField] private string testTitle = "Clue 1";
    [SerializeField, TextArea(4, 10)]
    private string testBody =
        "His Majesty no longer asks how to preserve his health.\n\n" +
        "He asks only whether death itself can be defeated.";
    [SerializeField] private bool isOpen;
    private bool justClosed; // NEW - guards against the same E press that closes it also reopening it
    private float canCloseAt;
    private float previousTimeScale = 1f;

    private GameObject currentPlayer;

    private bool justOpened;
    public bool IsClueOpen => documentRoot.activeSelf;
    public bool JustClosedThisFrame => justClosed; // NEW

    public static ClueDocumentUI Instance;

    private void Awake()
    {
        Instance = this;

        if (documentRoot != null)
            documentRoot.SetActive(false);
    }

    private void Update()
    {
        if (justOpened)
        {
            justOpened = false;
            return;
        }

        if (justClosed)
        {
            justClosed = false;
        }

        if (!isOpen)
        {
            return;
        }
        if (Time.unscaledTime < canCloseAt)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CloseClue();
        }
    }
    public void ShowClue(string clueTitle, string clueBody, GameObject player)
    {
        currentPlayer = player;

        if (currentPlayer != null)
        {
            currentPlayer.GetComponent<PlayerController>()
                         .SetMovementEnabled(false);
        }

        if (documentRoot == null || titleText == null || bodyText == null)
        {
            Debug.LogError("ClueDocumentUI references are not assigned.");
            return;
        }
        bool wasAlreadyOpen = isOpen;
        titleText.text = clueTitle;
        bodyText.text = clueBody;
        documentRoot.SetActive(true);
        
        isOpen = true;
        player.GetComponent<PlayerPickupAudio>()?.PlayPickupSound();

        justOpened = true;

        canCloseAt = Time.unscaledTime + closeInputDelay;
        if (pauseGame && !wasAlreadyOpen)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }
    }
    public void CloseClue()
    {
        if (!isOpen)
        {
            return;
        }
        documentRoot.SetActive(false);

        justClosed = true;
        isOpen = false;
        if (pauseGame)
        {
            Time.timeScale = previousTimeScale;
        }

        if (currentPlayer != null)
        {
            currentPlayer.GetComponent<PlayerController>()
                         .SetMovementEnabled(true);
        }

        currentPlayer = null;
    }
    private void OnDestroy()
    {
        if (isOpen && pauseGame)
        {
            Time.timeScale = previousTimeScale;
        }
    }
}