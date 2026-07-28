using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject victoryPanel;

    [Header("Scene")]
    [SerializeField] private string mainMenuSceneName = "MainMenuScene";

    private bool isVictoryShown;

    private void Awake()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

    public void ShowVictory()
    {
        if (isVictoryShown)
        {
            return;
        }

        isVictoryShown = true;

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;

        Debug.Log("Quit Game");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    [ContextMenu("Test Show Victory")]
    private void TestShowVictory()
    {
        ShowVictory();
    }
}
