using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class DamageFlashUIController : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private CanvasGroup damageCanvasGroup;

    [Header("Flash Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float peakAlpha = 0.45f;

    [Min(0f)]
    [SerializeField] private float fadeInDuration = 0.04f;

    [Min(0f)]
    [SerializeField] private float holdDuration = 0.04f;

    [Min(0f)]
    [SerializeField] private float fadeOutDuration = 0.3f;

    [Header("Temporary Keyboard Test")]
    [SerializeField] private bool enableKeyboardTest = true;
    [SerializeField] private KeyCode testKey = KeyCode.J;

    private Coroutine flashCoroutine;

    private void Awake()
    {
        if (damageCanvasGroup == null)
        {
            damageCanvasGroup = GetComponent<CanvasGroup>();
        }

        damageCanvasGroup.alpha = 0f;
        damageCanvasGroup.interactable = false;
        damageCanvasGroup.blocksRaycasts = false;
    }

    private void Update()
    {
        if (!enableKeyboardTest)
        {
            return;
        }

        if (Input.GetKeyDown(testKey))
        {
            PlayDamageFlash();
        }
    }

    public void PlayDamageFlash()
    {
        PlayDamageFlash(1f);
    }

    public void PlayDamageFlash(float intensity)
    {
        intensity = Mathf.Clamp01(intensity);

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(
            FlashAnimation(peakAlpha * intensity)
        );
    }

    public void StopDamageFlash()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }

        damageCanvasGroup.alpha = 0f;
    }

    private IEnumerator FlashAnimation(float targetAlpha)
    {
        float currentAlpha = damageCanvasGroup.alpha;

        yield return FadeCanvasGroup(
            currentAlpha,
            targetAlpha,
            fadeInDuration
        );

        if (holdDuration > 0f)
        {
            yield return new WaitForSecondsRealtime(
                holdDuration
            );
        }

        yield return FadeCanvasGroup(
            damageCanvasGroup.alpha,
            0f,
            fadeOutDuration
        );

        damageCanvasGroup.alpha = 0f;
        flashCoroutine = null;
    }

    private IEnumerator FadeCanvasGroup(
        float startAlpha,
        float targetAlpha,
        float duration
    )
    {
        if (duration <= 0f)
        {
            damageCanvasGroup.alpha = targetAlpha;
            yield break;
        }

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(
                elapsedTime / duration
            );

            float smoothProgress = Mathf.SmoothStep(
                0f,
                1f,
                progress
            );

            damageCanvasGroup.alpha = Mathf.Lerp(
                startAlpha,
                targetAlpha,
                smoothProgress
            );

            yield return null;
        }

        damageCanvasGroup.alpha = targetAlpha;
    }
}