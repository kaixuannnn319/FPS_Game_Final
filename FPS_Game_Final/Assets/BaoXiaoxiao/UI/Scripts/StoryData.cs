using UnityEngine;

[CreateAssetMenu(
    fileName = "Story_",
    menuName = "The Forgotten Island/Story Data"
)]
public class StoryData : ScriptableObject
{
    [TextArea(4, 12)]
    [SerializeField] private string content;

    public string Content => content;

    public void SetContent(string value)
    {
        content = value;
    }
}