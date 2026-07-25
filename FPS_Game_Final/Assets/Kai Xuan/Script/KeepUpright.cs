using UnityEngine;

public class KeepUpright : MonoBehaviour
{
    [SerializeField] private Transform wheel;

    private Quaternion startLocalRotation;

    void Start()
    {
        startLocalRotation = transform.localRotation;
    }

    void LateUpdate()
    {
        transform.localRotation =
            Quaternion.Inverse(wheel.localRotation) *
            startLocalRotation;
    }
}