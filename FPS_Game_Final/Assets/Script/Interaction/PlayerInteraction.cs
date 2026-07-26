using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        // NEW - while dialogue is open, don't process any interactions at all
        if (DialogueUI.Instance != null && DialogueUI.Instance.IsDialogueOpen) return;

        if (ClueUI.Instance != null && ClueUI.Instance.IsClueOpen) return;

        RaycastHit hit;
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out hit, 3f))
        {
            IInteractable target = hit.collider.GetComponentInParent<IInteractable>();

            if (target != null && Input.GetKeyDown(KeyCode.E))
            {
                target.Interact(gameObject);
            }
        }
    }
}