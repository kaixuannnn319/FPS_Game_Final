using UnityEngine;

public class PlatformParent : MonoBehaviour
{
    [SerializeField] private Transform platformRoot;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            other.transform.SetParent(platformRoot, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            other.transform.SetParent(null, true);
        }
    }
}