using UnityEngine;
using TMPro;

public class ClueUI : MonoBehaviour
{
    public static ClueUI Instance;

    [Header("UI References")]
    [SerializeField] private GameObject cluePanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI bodyText;

    private PlayerController player;

    public bool IsClueOpen { get; private set; }

    private void Awake()
    {
        Instance = this;
        cluePanel.SetActive(false);
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        // Press E again while the clue is open to close it and unfreeze the player
        if (IsClueOpen && Input.GetKeyDown(KeyCode.E))
        {
            CloseClue();
        }
    }

    public void ShowClue(string title, string body)
    {
        titleText.text = title;
        bodyText.text = body;
        cluePanel.SetActive(true);
        IsClueOpen = true;

        if (player != null)
        {
            player.SetMovementEnabled(false); // freeze player
        }
    }

    public void CloseClue()
    {
        cluePanel.SetActive(false);
        IsClueOpen = false;

        if (player != null)
        {
            player.SetMovementEnabled(true); // unfreeze player
        }
    }
}