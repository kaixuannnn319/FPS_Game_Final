using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject promptPanel;
    [SerializeField] private TMP_Text actionText;

    [Header("Display Settings")]
    [SerializeField] private bool hideOnStart = true;

    private void Awake()
    {
        if (hideOnStart)
        {
            HidePrompt();
        }
    }

    /// <summary>
    /// 显示交互提示，例如 PICK UP、READ、OPEN 或 TALK。
    /// </summary>
    public void ShowPrompt(string action)
    {
        if (promptPanel == null || actionText == null)
        {
            Debug.LogError(
                "InteractionPromptUI: PromptPanel or ActionText is not assigned.",
                this
            );
            return;
        }

        actionText.text = string.IsNullOrWhiteSpace(action)
            ? "INTERACT"
            : action.Trim().ToUpperInvariant();

        promptPanel.SetActive(true);
    }

    /// <summary>
    /// 隐藏交互提示。
    /// </summary>
    public void HidePrompt()
    {
        if (promptPanel != null)
        {
            promptPanel.SetActive(false);
        }
    }

    // 以下仅用于测试 UI。

    [ContextMenu("Test: Show PICK UP")]
    private void TestShowPickUp()
    {
        ShowPrompt("PICK UP");
    }

    [ContextMenu("Test: Show READ")]
    private void TestShowRead()
    {
        ShowPrompt("READ");
    }

    [ContextMenu("Test: Hide")]
    private void TestHide()
    {
        HidePrompt();
    }
}