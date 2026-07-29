using UnityEngine;
public class ClueScroll : MonoBehaviour, IInteractable
{
    [Header("Clue Content")]
    [SerializeField] private ClueData clueData;
    [SerializeField] private ClueDocumentUI clueUI;

    public void Interact(GameObject player)
    {
        Debug.Log("ClueScroll Interact");

        if (clueUI == null)
        {
            Debug.LogError("ClueDocumentUI is not assigned!");
            return;
        }

        Debug.Log("ClueUI Object = " + clueUI.gameObject.name);

        if (clueData == null)
        {
            Debug.LogError("ClueData is not assigned!");
            return;
        }

        if (ArchiveManager.Instance != null)
        {
            ArchiveManager.Instance.UnlockClue(clueData);
        }

        clueUI.ShowClue(
    clueData.ClueTitle,
    clueData.ClueContent,
    player);
    }
}