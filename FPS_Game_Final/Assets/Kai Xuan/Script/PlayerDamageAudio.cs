using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayerDamageAudio : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Damage")]
    [SerializeField] private AudioClip hurtClip;

    [SerializeField]
    [Range(0f, 1f)]
    private float hurtVolume = 0.6f;

    private AudioSource audioSource;
    private int previousHealth;

    [SerializeField] private AudioClip heartbeatClip;

    [SerializeField]
    private float heartbeatInterval = 1.2f;

    [SerializeField]
    private float heartbeatVolume = 1.5f;

    private bool heartbeatActive;
    private Coroutine heartbeatCoroutine;

    [SerializeField]
    [Range(0f, 1f)]
    private float lowHealthThreshold = 0.25f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (playerHealth == null)
            playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            previousHealth = playerHealth.GetCurrentHealth();
            playerHealth.OnHealthChange.AddListener(OnHealthChanged);
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChange.RemoveListener(OnHealthChanged);
    }

    private void OnHealthChanged(int currentHealth, int maxHealth)
    {
        // Hurt sound
        if (currentHealth < previousHealth)
        {
            audioSource.PlayOneShot(hurtClip, hurtVolume);
        }

        // Heartbeat
        float healthPercent = (float)currentHealth / maxHealth;

        if (healthPercent <= lowHealthThreshold)
        {
            if (!heartbeatActive)
            {
                heartbeatActive = true;
                heartbeatCoroutine = StartCoroutine(HeartbeatRoutine());
            }
        }
        else
        {
            if (heartbeatActive)
            {
                heartbeatActive = false;

                if (heartbeatCoroutine != null)
                {
                    StopCoroutine(heartbeatCoroutine);
                    heartbeatCoroutine = null;
                }
            }
        }

        previousHealth = currentHealth;
    }

    private IEnumerator HeartbeatRoutine()
    {
        while (heartbeatActive)
        {
            audioSource.PlayOneShot(heartbeatClip, heartbeatVolume);

            yield return new WaitForSeconds(heartbeatInterval);
        }
    }
}