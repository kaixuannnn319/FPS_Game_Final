using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
public class PlayerInteraction : MonoBehaviour
{
    private Camera playerCamera;

    [SerializeField] private InventoryToggle inventoryToggle;

    [SerializeField] private InteractionPromptUI interactionUI;

    void Start()
    {
        playerCamera = Camera.main;
    }
    void Update()
    {
        // while dialogue is open, don't process any interactions at all
        if (DialogueUI.Instance != null && DialogueUI.Instance.IsDialogueOpen)
        {
            interactionUI.HidePrompt();
            return;
        }

        // NEW - also skip this frame if dialogue JUST closed, so the same
        // E press that closed it can't immediately reopen it (fixes the
        // infinite dialogue loop bug)
        if (DialogueUI.Instance != null && DialogueUI.Instance.JustClosedThisFrame)
        {
            interactionUI.HidePrompt();
            return;
        }

        // while a clue is open, don't process any interactions at all
        if (ClueUI.Instance != null && ClueUI.Instance.IsClueOpen)
        {
            interactionUI.HidePrompt();
            return;
        }

        if (inventoryToggle != null && inventoryToggle.IsOpen)
        {
            interactionUI.HidePrompt();
            return;
        }

        RaycastHit hit;
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out hit, 3f))
        {
            IInteractable target = hit.collider.GetComponentInParent<IInteractable>();

            if (target != null)
            {
                interactionUI.ShowPrompt(GetInteractionText(target));

                if (Input.GetKeyDown(KeyCode.E))
                {
                    target.Interact(gameObject);
                }
            }
            else
            {
                interactionUI.HidePrompt();
            }
        }
        else
        {
            interactionUI.HidePrompt();
        }
    }

    private string GetInteractionText(IInteractable target)
    {
        if (target is NPCDialogue) return "TALK";
        if (target is ClueScroll) return "READ";
        if (target is Door) return "OPEN";
        if (target is BuffPickup) return "PICK UP";
        if (target is HealthPotionPickup) return "PICK UP";
        if (target is WeaponPickup) return "PICK UP";
        if (target is BulletPickup) return "PICK UP";

        return "INTERACT";
    }
}