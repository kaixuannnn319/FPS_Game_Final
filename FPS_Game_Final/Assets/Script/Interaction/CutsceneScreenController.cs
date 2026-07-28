using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneScreenController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image cutsceneImage;

    [Header("Cutscene Content")]
    [Tooltip("Each sprite is one page/illustration, shown in order as the player presses E.")]
    [SerializeField] private Sprite[] pages;

    [Header("Scene Transition")]
    [Tooltip("Exact scene name to load once the last page is closed.")]
    [SerializeField] private string nextSceneName;

    private int currentIndex;

    private void Start()
    {
        currentIndex = 0;

        if (pages == null || pages.Length == 0)
        {
            Debug.LogError("CutsceneScreenController: no pages assigned.");
            return;
        }

        cutsceneImage.sprite = pages[currentIndex];
    }

    private void Update()
    {
        if (pages == null || pages.Length == 0) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentIndex++;

            if (currentIndex < pages.Length)
            {
                cutsceneImage.sprite = pages[currentIndex];
            }
            else
            {
                if (string.IsNullOrEmpty(nextSceneName))
                {
                    Debug.LogError("CutsceneScreenController: Next Scene Name is not set.");
                    return;
                }

                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}