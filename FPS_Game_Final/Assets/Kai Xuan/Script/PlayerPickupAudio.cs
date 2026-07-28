using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerPickupAudio : MonoBehaviour
{
    [SerializeField] private AudioClip pickupClip;
    [SerializeField] private float pickupVolume = 1f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayPickupSound()
    {
        if (pickupClip != null)
        {
            audioSource.PlayOneShot(pickupClip, pickupVolume);
        }
    }
}