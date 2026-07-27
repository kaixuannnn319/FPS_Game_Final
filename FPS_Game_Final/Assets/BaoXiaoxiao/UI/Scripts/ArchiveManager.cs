using System;
using System.Collections.Generic;
using UnityEngine;

public class ArchiveManager : MonoBehaviour
{
    public static ArchiveManager Instance { get; private set; }

    [SerializeField] private List<StoryData> unlockedStories = new();

    public event Action ArchiveChanged;

    public IReadOnlyList<StoryData> UnlockedStories => unlockedStories;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        unlockedStories.RemoveAll(story => story == null);
    }

    public bool UnlockStory(StoryData story)
    {
        if (story == null)
        {
            Debug.LogError("ArchiveManager: StoryData is missing.", this);
            return false;
        }

        if (unlockedStories.Contains(story))
        {
            return false;
        }

        unlockedStories.Add(story);
        ArchiveChanged?.Invoke();

        return true;
    }

    public bool IsStoryUnlocked(StoryData story)
    {
        return story != null && unlockedStories.Contains(story);
    }

    public void ClearArchive()
    {
        unlockedStories.Clear();
        ArchiveChanged?.Invoke();
    }
}
