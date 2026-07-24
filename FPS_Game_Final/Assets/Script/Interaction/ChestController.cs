using UnityEngine;
using System.Collections;

public class ChestController : MonoBehaviour, IInteractable
{
    [SerializeField] private float lidOpenAngle = 100f;
    [SerializeField] private float openDuration = 0.2f;
    [SerializeField] private Transform lid;
    [SerializeField] private GameObject[] itemsInside; // whatever items sit inside this chest

    private bool isOpen = false;
    private Quaternion lidClosedRotation;
    private Quaternion lidOpenRotation;

    void Start()
    {
        if (lid != null)
        {
            lidClosedRotation = lid.localRotation;
            lidOpenRotation = lidClosedRotation * Quaternion.Euler(0f, 0f, lidOpenAngle);
        }
    }

    public void Interact(GameObject player)
    {
        if (isOpen) return;

        isOpen = true;
        Debug.Log("Chest opened!");

        if (lid != null)
        {
            StartCoroutine(OpenLid());
        }

        RevealItems();
    }

    private IEnumerator OpenLid()
    {
        float elapsed = 0f;

        while (elapsed < openDuration)
        {
            lid.localRotation = Quaternion.Slerp(lidClosedRotation, lidOpenRotation, elapsed / openDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        lid.localRotation = lidOpenRotation;
    }

    private void RevealItems()
    {
        foreach (GameObject item in itemsInside)
        {
            if (item != null)
            {
                item.SetActive(true);

                FloatOutOfChest floatScript = item.GetComponent<FloatOutOfChest>();
                if (floatScript != null)
                {
                    floatScript.PlayFloatAnimation();
                }
            }
        }
    }
}