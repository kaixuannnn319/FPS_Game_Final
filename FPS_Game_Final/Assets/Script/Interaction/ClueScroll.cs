using UnityEngine;

public class ClueScroll : MonoBehaviour, IInteractable
{
    [Header("Clue Content")]
    [SerializeField] private string clueTitle;

    [SerializeField] private StoryData clueContent;

    [SerializeField] private StoryUIController storyUI;

    public void Interact(GameObject player)
    {
        if (storyUI == null)
        {
            Debug.LogError("StoryUIController is not assigned!");
            return;
        }

        if (clueContent == null)
        {
            Debug.LogError("StoryData is not assigned!");
            return;
        }

        storyUI.OpenStory(clueContent.Content);
    }
}