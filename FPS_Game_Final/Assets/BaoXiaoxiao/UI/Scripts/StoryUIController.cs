using TMPro;
using UnityEngine;
public class StoryUIController : MonoBehaviour
{
    [SerializeField] private GameObject documentUI;
    [SerializeField] private TMP_Text storyBody;

    private string[] currentPages;
    private int currentIndex;
    private GameObject currentPlayer;

    private bool isOpen;
    private bool justOpened;
    private bool justClosed; // guards against same-frame reopen, same fix as DialogueUI

    public bool IsStoryOpen => isOpen;
    public bool JustClosedThisFrame => justClosed;

    private void Awake()
    {
        CloseStoryInternal();
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

        if (!isOpen) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentIndex++;
            if (currentIndex < currentPages.Length)
            {
                storyBody.text = currentPages[currentIndex];
            }
            else
            {
                CloseStory();
            }
        }
    }

    /// <summary>
    /// Opens the storybook UI showing the first page, freezes player movement,
    /// and advances one page per E press until the last page, where E closes it.
    /// </summary>
    public void OpenStory(string[] pages, GameObject player)
    {
        if (documentUI == null || storyBody == null)
        {
            Debug.LogError("Story UI references are missing.");
            return;
        }

        if (pages == null || pages.Length == 0)
        {
            Debug.LogError("StoryUIController: pages array is empty.");
            return;
        }

        currentPages = pages;
        currentIndex = 0;
        currentPlayer = player;

        storyBody.text = currentPages[currentIndex];
        documentUI.SetActive(true);
        isOpen = true;
        justOpened = true;
        player.GetComponent<PlayerPickupAudio>()?.PlayPickupSound();

        if (currentPlayer != null)
        {
            currentPlayer.GetComponent<PlayerController>().SetMovementEnabled(false);
        }
    }

    /// <summary>
    /// Convenience overload for a single-page story - behaves exactly like
    /// the original single-string version (press E once to close).
    /// </summary>
    public void OpenStory(string content, GameObject player)
    {
        OpenStory(new string[] { content }, player);
    }

    public void CloseStory()
    {
        CloseStoryInternal();
        justClosed = true;

        if (currentPlayer != null)
        {
            currentPlayer.GetComponent<PlayerController>().SetMovementEnabled(true);
        }

        currentPlayer = null;
    }

    private void CloseStoryInternal()
    {
        if (documentUI != null)
        {
            documentUI.SetActive(false);
        }
        isOpen = false;
    }
}