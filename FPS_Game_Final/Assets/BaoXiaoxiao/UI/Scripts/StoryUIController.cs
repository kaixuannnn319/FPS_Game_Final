using TMPro;
using UnityEngine;

public class StoryUIController : MonoBehaviour
{
    [SerializeField] private GameObject documentUI;
    [SerializeField] private TMP_Text storyBody;
    [SerializeField] private bool enableKeyboardTest = true;

    [TextArea(3, 10)]
    [SerializeField]
    private string sampleStory =
        "His Majesty no longer asks how to preserve his health.\n\n" +
        "He asks only whether death itself can be defeated.";

    private bool isOpen;

    public bool IsStoryOpen => isOpen;

    private void Awake()
    {
        CloseStory();
    }

    private void Update()
    {
        Debug.Log("StoryUI Update");

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("P Pressed");
            OpenStory("TEST");
        }

        if (!isOpen)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            CloseStory();
        }
    }

    public void OpenStory(string content)
    {
        if (documentUI == null || storyBody == null)
        {
            Debug.LogError("Story UI references are missing.");
            return;
        }

        storyBody.text = content;
        documentUI.SetActive(true);
        isOpen = true;
    }

    public void CloseStory()
    {
        if (documentUI != null)
        {
            documentUI.SetActive(false);
        }

        isOpen = false;
    }
}
