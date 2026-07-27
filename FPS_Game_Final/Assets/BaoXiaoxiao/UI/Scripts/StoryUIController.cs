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

    private void Awake()
    {
        CloseStory();
    }

    private void Update()
    {
        if (!enableKeyboardTest)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isOpen)
            {
                CloseStory();
            }
            else
            {
                OpenStory(sampleStory);
            }
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
