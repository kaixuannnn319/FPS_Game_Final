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
    private Animator currentAnimator; // NEW - tracks which NPC is talking
    private bool justOpened;

    public bool IsDialogueOpen => dialoguePanel.activeSelf;

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
        currentAnimator = speakerAnimator; // NEW

        dialoguePanel.SetActive(true);
        dialogueText.text = currentLines[currentIndex];

        currentPlayer.GetComponent<PlayerController>().SetMovementEnabled(false);

        if (currentAnimator != null) currentAnimator.SetBool("IsTalking", true); // NEW

        justOpened = true;
    }

    private void Update()
    {
        if (justOpened)
        {
            justOpened = false;
            return;
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
        currentPlayer.GetComponent<PlayerController>().SetMovementEnabled(true);

        if (currentAnimator != null) currentAnimator.SetBool("IsTalking", false);
        currentAnimator = null;
    }
}