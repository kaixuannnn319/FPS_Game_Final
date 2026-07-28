using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EndingCutsceneSlide
{
    public Sprite image;

    [TextArea(3, 6)]
    public string subtitle;

    [Min(0.5f)]
    public float duration = 5f;
}

public class EndingCutsceneController : MonoBehaviour
{
    [Header("Cutscene UI")]
    [SerializeField] private GameObject cutsceneRoot;
    [SerializeField] private Image cutsceneImage;
    [SerializeField] private TMP_Text subtitleText;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Cutscene Slides")]
    [SerializeField] private EndingCutsceneSlide[] slides;

    [Header("Timing")]
    [SerializeField] private float fadeDuration = 0.6f;

    [Header("Victory")]
    [SerializeField] private GameObject victoryPanel;

    [Header("Testing")]
    [SerializeField] private bool playOnStart;

    private bool isPlaying;

    private void Awake()
    {
        if (cutsceneRoot != null)
        {
            cutsceneRoot.SetActive(false);
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

    private void Start()
    {
        if (playOnStart)
        {
            StartCutscene();
        }
    }

    public void StartCutscene()
    {
        if (isPlaying)
        {
            return;
        }

        if (cutsceneRoot == null ||
            cutsceneImage == null ||
            subtitleText == null ||
            canvasGroup == null)
        {
            Debug.LogError("Ending cutscene UI references are incomplete.");
            return;
        }

        if (slides == null || slides.Length == 0)
        {
            Debug.LogWarning("No ending cutscene slides have been assigned.");
            return;
        }

        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        isPlaying = true;

        cutsceneRoot.SetActive(true);
        cutsceneRoot.transform.SetAsLastSibling();
        canvasGroup.alpha = 0f;

        for (int i = 0; i < slides.Length; i++)
        {
            EndingCutsceneSlide slide = slides[i];

            if (slide.image == null)
            {
                Debug.LogWarning($"Ending cutscene slide {i} has no image.");
                continue;
            }

            cutsceneImage.sprite = slide.image;
            subtitleText.text = slide.subtitle;

            yield return FadeCanvas(1f);
            yield return new WaitForSecondsRealtime(slide.duration);
            yield return FadeCanvas(0f);
        }

        cutsceneRoot.SetActive(false);

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            victoryPanel.transform.SetAsLastSibling();
        }

        isPlaying = false;
    }

    private IEnumerator FadeCanvas(float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        if (fadeDuration <= 0f)
        {
            canvasGroup.alpha = targetAlpha;
            yield break;
        }

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            canvasGroup.alpha = Mathf.Lerp(
                startAlpha,
                targetAlpha,
                elapsedTime / fadeDuration
            );

            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }
}