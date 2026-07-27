using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArchiveUIController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform entryContainer;
    [SerializeField] private Button entryButtonPrefab;
    [SerializeField] private TMP_Text detailText;

    [Header("Display Settings")]
    [SerializeField] private string emptyDetailText = "Select a record to view its contents.";

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

        if (detailText != null)
        {
            detailText.text = emptyDetailText;
        }

        if (ArchiveManager.Instance == null)
        {
            return;
        }

        IReadOnlyList<StoryData> stories =
            ArchiveManager.Instance.UnlockedStories;

        foreach (StoryData story in stories)
        {
            if (story == null)
            {
                continue;
            }

            CreateEntry(story);
        }
    }

    private void CreateEntry(StoryData story)
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
            buttonText.text = GetRecordLabel(story);
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(
            () => ShowStoryDetails(story)
        );

        spawnedButtons.Add(button);
    }

    private void ShowStoryDetails(StoryData story)
    {
        if (detailText == null || story == null)
        {
            return;
        }

        detailText.text = story.Content;
    }

    private string GetRecordLabel(StoryData story)
    {
        string assetName = story.name;
        int underscoreIndex = assetName.LastIndexOf('_');

        if (underscoreIndex >= 0 &&
            underscoreIndex < assetName.Length - 1)
        {
            string numberText =
                assetName[(underscoreIndex + 1)..];

            if (int.TryParse(numberText, out int number))
            {
                return $"RECORD {number:00}";
            }
        }

        return assetName
            .Replace('_', ' ')
            .ToUpperInvariant();
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