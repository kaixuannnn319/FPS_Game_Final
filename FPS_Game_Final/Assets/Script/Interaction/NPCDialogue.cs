using UnityEngine;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] private string[] dialogueLines;

    public void Interact(GameObject player)
    {
        Debug.Log("NPC Interact triggered on: " + gameObject.name);
        DialogueUI.Instance.ShowDialogue(dialogueLines, player);
    }
}