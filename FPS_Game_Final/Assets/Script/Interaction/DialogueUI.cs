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

    private void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
    }

    public void ShowDialogue(string[] lines, GameObject player)
    {
        currentLines = lines;
        currentIndex = 0;
        currentPlayer = player;

        dialoguePanel.SetActive(true);
        dialogueText.text = currentLines[currentIndex];

        currentPlayer.GetComponent<PlayerController>().SetMovementEnabled(false);
    }

    private void Update()
    {
        if (dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            currentIndex++;

            if (currentIndex < currentLines.Length)
            {
                dialogueText.text = currentLines[currentIndex];
            }
            else
            {
                dialoguePanel.SetActive(false);
                currentPlayer.GetComponent<PlayerController>().SetMovementEnabled(true);
            }
        }
    }
}