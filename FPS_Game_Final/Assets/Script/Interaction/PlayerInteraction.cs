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