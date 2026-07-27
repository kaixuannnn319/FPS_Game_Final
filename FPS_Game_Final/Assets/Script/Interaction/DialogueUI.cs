using UnityEngine;
using TMPro;
public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    private string[] currentLines;
    private int currentIndex;
    private GameObject currentPlayer;
    private Animator currentAnimator; // tracks which NPC is talking
    private bool justOpened;
    private bool justClosed; // NEW - guards against same-frame reopen race condition
    public bool IsDialogueOpen => dialoguePanel.activeSelf;
    public bool JustClosedThisFrame => justClosed; // NEW

    private PlayerController playerController;

    private void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
    }
    public void ShowDialogue(string[] lines, GameObject player, Animator speakerAnimator)
    {
        currentLines = lines;
        currentIndex = 0;
        currentPlayer = player;
        currentAnimator = speakerAnimator;
        dialoguePanel.SetActive(true);
        dialogueText.text = currentLines[currentIndex];

        playerController = player.GetComponent<PlayerController>();
        playerController.SetMovementEnabled(false);

        if (currentAnimator != null) currentAnimator.SetBool("IsTalking", true);
        justOpened = true;
    }
    private void Update()
    {
        if (justOpened)
        {
            justOpened = false;
            return;
        }

        // NEW - consume the "just closed" flag one frame after it was set,
        // so PlayerInteraction has a chance to read it first this same frame
        if (justClosed)
        {
            justClosed = false;
        }

        if (!dialoguePanel.activeSelf) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentIndex++;
            if (currentIndex < currentLines.Length)
            {
                dialogueText.text = currentLines[currentIndex];
            }
            else
            {
                CloseDialogue();
            }
        }
    }
    private void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        justClosed = true; // NEW - blocks re-Interact on this same E press

        playerController.SetMovementEnabled(true);

        if (currentAnimator != null) currentAnimator.SetBool("IsTalking", false);
        currentAnimator = null;
    }
}