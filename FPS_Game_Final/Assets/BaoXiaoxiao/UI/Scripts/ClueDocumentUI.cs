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
    private bool isOpen;
    private bool justClosed; // NEW - guards against the same E press that closes it also reopening it
    private float canCloseAt;
    private float previousTimeScale = 1f;
    public bool IsOpen => isOpen;
    public bool JustClosedThisFrame => justClosed; // NEW
    private void Awake()
    {
        if (documentRoot != null)
        {
            documentRoot.SetActive(false);
        }
    }
    private void Update()
    {
        if (justClosed)
        {
            justClosed = false;
        }

        if (!isOpen)
        {
            if (enableTestKey && Input.GetKeyDown(testOpenKey))
            {
                ShowClue(testTitle, testBody);
            }
            return;
        }
        if (Time.unscaledTime < canCloseAt)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            CloseClue();
        }
    }
    public void ShowClue(string clueTitle, string clueBody)
    {
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
        isOpen = false;
        justClosed = true; // NEW
        if (pauseGame)
        {
            Time.timeScale = previousTimeScale;
        }
    }
    private void OnDestroy()
    {
        if (isOpen && pauseGame)
        {
            Time.timeScale = previousTimeScale;
        }
    }
}