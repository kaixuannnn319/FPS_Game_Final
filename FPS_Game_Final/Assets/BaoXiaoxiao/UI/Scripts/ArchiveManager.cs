using System;
using System.Collections.Generic;
using UnityEngine;

public class ArchiveManager : MonoBehaviour
{
    public static ArchiveManager Instance { get; private set; }

    [SerializeField] private List<ClueData> unlockedClues = new();

    public event Action ArchiveChanged;

    public IReadOnlyList<ClueData> UnlockedClues => unlockedClues;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        unlockedClues.RemoveAll(clue => clue == null);
    }

    public bool UnlockClue(ClueData clue)
    {
        if (clue == null)
        {
            Debug.LogError("ArchiveManager: ClueData is missing.", this);
            return false;
        }

        if (ContainsClue(clue))
        {
            return false;
        }

        unlockedClues.Add(clue);
        ArchiveChanged?.Invoke();

        return true;
    }

    public bool IsClueUnlocked(ClueData clue)
    {
        return clue != null && ContainsClue(clue);
    }

    public void ClearArchive()
    {
        unlockedClues.Clear();
        ArchiveChanged?.Invoke();
    }

    private bool ContainsClue(ClueData clue)
    {
        foreach (ClueData unlockedClue in unlockedClues)
        {
            if (unlockedClue == null)
            {
                continue;
            }

            if (unlockedClue == clue)
            {
                return true;
            }

            if (!string.IsNullOrWhiteSpace(clue.ClueId) &&
                unlockedClue.ClueId == clue.ClueId)
            {
                return true;
            }
        }

        return false;
    }
}