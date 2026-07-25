using UnityEngine;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] private string[] dialogueLines;
    [SerializeField] private Animator animator;

    public void Interact(GameObject player)
    {
        DialogueUI.Instance.ShowDialogue(dialogueLines, player, animator);
    }
}