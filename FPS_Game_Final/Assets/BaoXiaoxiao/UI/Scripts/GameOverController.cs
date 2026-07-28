using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("Scene")]
    [SerializeField] private string mainMenuSceneName = "MainMenuScene";

    private bool isGameOver;

    private void Awake()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void ShowGameOver()
    {
        if (isGameOver)
        {
            return;
        }

        isGameOver = true;

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(mainMenuSceneName);
    }

    [ContextMenu("Test Show Game Over")]
    private void TestShowGameOver()
    {
        ShowGameOver();
    }
}
