using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField] private string requiredKeyID;
    [SerializeField] private float slideDistance = 2f;
    [SerializeField] private float slideDuration = 1f;

    private bool isOpen = false;
    private Vector3 closedPosition;
    private Vector3 openPosition;

    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + transform.right * slideDistance;
    }

    public void Interact(GameObject player)
    {
        if (isOpen) return;

        if (PlayerInventoryStub.Instance.HasKey(requiredKeyID))
        {
            isOpen = true;
            Debug.Log("Door opened with " + requiredKeyID);
            StartCoroutine(SlideDoor());
        }
        else
        {
            Debug.Log("Locked - need " + requiredKeyID);
        }
    }

    private IEnumerator SlideDoor()
    {
        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            transform.position = Vector3.Lerp(closedPosition, openPosition, elapsed / slideDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = openPosition;

        GetComponent<Collider>().enabled = false;
    }
}