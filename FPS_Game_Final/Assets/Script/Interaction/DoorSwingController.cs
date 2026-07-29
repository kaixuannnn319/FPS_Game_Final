using UnityEngine;
using System.Collections;

public class DoorSwingController : MonoBehaviour, IInteractable
{
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float swingDuration = 1f;
    [SerializeField] private bool openInward = true;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    private bool isOpen = false;
    private bool isMoving = false;
    public bool IsOpen => isOpen;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        closedRotation = transform.localRotation;
        float angle = openInward ? openAngle : -openAngle;
        openRotation = closedRotation * Quaternion.Euler(0f, angle, 0f);
    }

    public void Interact(GameObject player)
    {
        if (isMoving) return;

        if (!isOpen)
        {
            audioSource.PlayOneShot(openSound);

            StartCoroutine(Swing(closedRotation, openRotation));
            isOpen = true;
        }
        else
        {
            audioSource.PlayOneShot(closeSound, 0.5f);

            StartCoroutine(Swing(openRotation, closedRotation));
            isOpen = false;
        }
    }

    private IEnumerator Swing(Quaternion from, Quaternion to)
    {
        isMoving = true;
        float elapsed = 0f;

        while (elapsed < swingDuration)
        {
            transform.localRotation = Quaternion.Slerp(from, to, elapsed / swingDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = to;
        isMoving = false;
    }
}