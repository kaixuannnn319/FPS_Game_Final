using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject promptPanel;
    [SerializeField] private TMP_Text actionText;

    [Header("Display Settings")]
    [SerializeField] private bool hideOnStart = true;
    [SerializeField] private string defaultAction = "INTERACT";

    public bool IsVisible =>
        promptPanel != null && promptPanel.activeSelf;

    private void Awake()
    {
        if (hideOnStart)
        {
            HidePrompt();
        }
    }

    /// <summary>
    /// Displays the interaction prompt with the provided action text.
    /// Examples: PICK UP, READ, OPEN, TALK, UNLOCK.
    /// </summary>
    public void ShowPrompt(string action)
    {
        if (!ValidateReferences())
        {
            return;
        }

        actionText.text = string.IsNullOrWhiteSpace(action)
            ? defaultAction.ToUpperInvariant()
            : action.Trim().ToUpperInvariant();

        promptPanel.SetActive(true);
    }

    /// <summary>
    /// Hides the interaction prompt.
    /// </summary>
    public void HidePrompt()
    {
        if (promptPanel != null)
        {
            promptPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Updates the action text without changing the current visibility.
    /// </summary>
    public void SetActionText(string action)
    {
        if (actionText == null)
        {
            Debug.LogError(
                "InteractionPromptUI: ActionText is not assigned.",
                this
            );
            return;
        }

        actionText.text = string.IsNullOrWhiteSpace(action)
            ? defaultAction.ToUpperInvariant()
            : action.Trim().ToUpperInvariant();
    }

    private bool ValidateReferences()
    {
        if (promptPanel == null)
        {
            Debug.LogError(
                "InteractionPromptUI: PromptPanel is not assigned.",
                this
            );
            return false;
        }

        if (actionText == null)
        {
            Debug.LogError(
                "InteractionPromptUI: ActionText is not assigned.",
                this
            );
            return false;
        }

        return true;
    }

    [ContextMenu("Test/Show INTERACT")]
    private void TestShowInteract()
    {
        ShowPrompt("INTERACT");
    }

    [ContextMenu("Test/Show PICK UP")]
    private void TestShowPickUp()
    {
        ShowPrompt("PICK UP");
    }

    [ContextMenu("Test/Show READ")]
    private void TestShowRead()
    {
        ShowPrompt("READ");
    }

    [ContextMenu("Test/Show TALK")]
    private void TestShowTalk()
    {
        ShowPrompt("TALK");
    }

    [ContextMenu("Test/Show OPEN")]
    private void TestShowOpen()
    {
        ShowPrompt("OPEN");
    }

    [ContextMenu("Test/Show UNLOCK")]
    private void TestShowUnlock()
    {
        ShowPrompt("UNLOCK");
    }

    [ContextMenu("Test/Show UPGRADE")]
    private void TestShowUpgrade()
    {
        ShowPrompt("UPGRADE");
    }

    [ContextMenu("Test/Show ACTIVATE")]
    private void TestShowActivate()
    {
        ShowPrompt("ACTIVATE");
    }

    [ContextMenu("Test/Show PLACE SEAL")]
    private void TestShowPlaceSeal()
    {
        ShowPrompt("PLACE SEAL");
    }

    [ContextMenu("Test/Show INSPECT")]
    private void TestShowInspect()
    {
        ShowPrompt("INSPECT");
    }

    [ContextMenu("Test/Show BOARD")]
    private void TestShowBoard()
    {
        ShowPrompt("BOARD");
    }

    [ContextMenu("Test/Hide")]
    private void TestHide()
    {
        HidePrompt();
    }
}