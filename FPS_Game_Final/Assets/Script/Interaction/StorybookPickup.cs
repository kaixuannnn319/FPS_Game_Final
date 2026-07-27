using UnityEngine;

public class StorybookPickup : MonoBehaviour, IInteractable
{
    [Header("Story Content")]
    [Tooltip("Each StoryData asset is one page - shown in order as the player presses E.")]
    [SerializeField] private StoryData[] storyPages;

    public void Interact(GameObject player)
    {
        StoryUIController storyUI = FindObjectOfType<StoryUIController>();

        if (storyUI == null)
        {
            Debug.LogError("StoryUIController not found in scene!");
            return;
        }

        if (storyPages == null || storyPages.Length == 0)
        {
            Debug.LogError("StorybookPickup: no StoryData assigned on " + gameObject.name);
            return;
        }

        string[] pages = new string[storyPages.Length];
        for (int i = 0; i < storyPages.Length; i++)
        {
            pages[i] = storyPages[i].Content;
        }

        storyUI.OpenStory(pages, player);
    }
}