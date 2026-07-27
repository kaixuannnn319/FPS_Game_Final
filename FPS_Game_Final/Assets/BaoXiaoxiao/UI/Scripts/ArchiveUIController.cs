using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArchiveUIController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform entryContainer;
    [SerializeField] private Button entryButtonPrefab;
    [SerializeField] private TMP_Text detailTitleText;
    [SerializeField] private TMP_Text detailText;

    [Header("Display Settings")]
    [SerializeField] private string emptyDetailTitle = "";
    [SerializeField]
    private string emptyDetailText =
        "Select a clue to view its details.";

    private readonly List<Button> spawnedButtons = new();

    private void OnEnable()
    {
        TrySubscribe();
        RefreshArchive();
    }

    private void Start()
    {
        TrySubscribe();
        RefreshArchive();
    }

    private void OnDisable()
    {
        if (ArchiveManager.Instance != null)
        {
            ArchiveManager.Instance.ArchiveChanged -= RefreshArchive;
        }
    }

    private void TrySubscribe()
    {
        if (ArchiveManager.Instance == null)
        {
            return;
        }

        ArchiveManager.Instance.ArchiveChanged -= RefreshArchive;
        ArchiveManager.Instance.ArchiveChanged += RefreshArchive;
    }

    public void RefreshArchive()
    {
        ClearButtons();

        if (detailTitleText != null)
        {
            detailTitleText.text = emptyDetailTitle;
        }

        if (detailText != null)
        {
            detailText.text = emptyDetailText;
        }

        if (ArchiveManager.Instance == null)
        {
            return;
        }

        IReadOnlyList<ClueData> clues =
            ArchiveManager.Instance.UnlockedClues;

        foreach (ClueData clue in clues)
        {
            if (clue == null)
            {
                continue;
            }

            CreateEntry(clue);
        }
    }

    private void CreateEntry(ClueData clue)
    {
        if (entryContainer == null ||
            entryButtonPrefab == null ||
            detailText == null)
        {
            Debug.LogError(
                "ArchiveUIController: UI references are missing.",
                this
            );

            return;
        }

        Button button = Instantiate(
            entryButtonPrefab,
            entryContainer
        );

        button.gameObject.SetActive(true);

        TMP_Text buttonText =
            button.GetComponentInChildren<TMP_Text>();

        if (buttonText != null)
        {
            buttonText.text = GetClueLabel(clue);
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(
            () => ShowClueDetails(clue)
        );

        spawnedButtons.Add(button);
    }

    private void ShowClueDetails(ClueData clue)
    {
        if (clue == null)
        {
            return;
        }

        if (detailTitleText != null)
        {
            detailTitleText.text = GetClueLabel(clue);
        }

        if (detailText != null)
        {
            detailText.text = clue.ClueContent;
        }
    }

    private string GetClueLabel(ClueData clue)
    {
        if (clue == null)
        {
            return "";
        }

        if (!string.IsNullOrWhiteSpace(clue.ClueTitle))
        {
            return clue.ClueTitle;
        }

        return clue.name.Replace('_', ' ');
    }

    private void ClearButtons()
    {
        foreach (Button button in spawnedButtons)
        {
            if (button != null)
            {
                Destroy(button.gameObject);
            }
        }

        spawnedButtons.Clear();

        if (entryContainer == null)
        {
            return;
        }

        for (int i = entryContainer.childCount - 1; i >= 0; i--)
        {
            Transform child = entryContainer.GetChild(i);

            if (child != null)
            {
                Destroy(child.gameObject);
            }
        }
    }
}