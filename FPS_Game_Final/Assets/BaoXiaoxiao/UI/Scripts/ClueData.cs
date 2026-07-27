using UnityEngine;
[CreateAssetMenu(fileName = "NewClueData", menuName = "Game Data/Clue Data")]
public class ClueData : ScriptableObject
{
    [SerializeField] private string clueId;
    [SerializeField] private string clueTitle;
    [SerializeField, TextArea(5, 15)]
    private string clueContent;
    public string ClueId => clueId;
    public string ClueTitle => clueTitle;
    public string ClueContent => clueContent;
}