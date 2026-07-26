using UnityEngine;

public class ClueScroll : MonoBehaviour, IInteractable
{
    [Header("Clue Content")]
    [SerializeField] private string clueTitle;

    [TextArea(3, 10)]
    [SerializeField] private string clueText;

    public void Interact(GameObject player)
    {
        if (ClueUI.Instance == null)
        {
            Debug.LogError("ClueUI.Instance not found in scene!");
            return;
        }

        ClueUI.Instance.ShowClue(clueTitle, clueText);
    }
}