using UnityEngine;
public class ClueScroll : MonoBehaviour, IInteractable
{
    [Header("Clue Content")]
    [SerializeField] private ClueData clueData;
    [SerializeField] private ClueDocumentUI clueUI;

    public void Interact(GameObject player)
    {
        if (clueUI == null)
        {
            Debug.LogError("ClueDocumentUI is not assigned!");
            return;
        }
        if (clueData == null)
        {
            Debug.LogError("ClueData is not assigned!");
            return;
        }

        clueUI.ShowClue(clueData.ClueTitle, clueData.ClueContent);
    }
}