using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [Header("Pause Panels")]
    [SerializeField] private GameObject pauseDimBackground;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseSettingsPanel;
    [SerializeField] private GameObject pauseControlsPanel;

    [Header("Settings Text")]
    [SerializeField] private TMP_Text audioButtonText;
    [SerializeField] private TMP_Text fullscreenButtonText;

    [Header("Controls Pages")]
    [SerializeField] private GameObject keyboardImage;
    [SerializeField] private GameObject mouseImage;
    [SerializeField] private GameObject controlsNextButton;

    [Header("Scene")]
    [SerializeField] private string mainMenuSceneName = "MainMenuScene";

    [Header("Input")]
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;

    private const string AudioKey = "AudioEnabled";
    private const string FullscreenKey = "FullscreenEnabled";

    private bool isPaused;
    private bool audioEnabled;
    private bool fullscreenEnabled;

    private float previousTimeScale = 1f;
    private CursorLockMode previousCursorLockMode;
    private bool previousCursorVisible;

    private void Start()
    {
        audioEnabled = PlayerPrefs.GetInt(AudioKey, 1) == 1;
        fullscreenEnabled = PlayerPrefs.GetInt(
            FullscreenKey,
            Screen.fullScreen ? 1 : 0
        ) == 1;

        ApplyAudioSetting();
        ApplyFullscreenSetting();
        HideAllPauseUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            HandlePauseInput();
        }
    }

    public void ResumeGame()
    {
        if (!isPaused)
        {
            return;
        }

        HideAllPauseUI();
        isPaused = false;

        Time.timeScale = previousTimeScale;
        Cursor.lockState = previousCursorLockMode;
        Cursor.visible = previousCursorVisible;
    }

    public void OpenPauseSettings()
    {
        if (!isPaused)
        {
            return;
        }

        SetPauseUIState(true, false, true, false);
        UpdateSettingTexts();
    }

    public void ClosePauseSettings()
    {
        if (!isPaused)
        {
            return;
        }

        ShowPauseMain();
    }

    public void OpenPauseControls()
    {
        if (!isPaused)
        {
            return;
        }

        SetPauseUIState(true, false, false, true);
        ShowKeyboardControls();
    }

    public void ClosePauseControls()
    {
        if (!isPaused)
        {
            return;
        }

        ShowPauseMain();
    }

    public void ShowMouseControls()
    {
        if (keyboardImage != null)
        {
            keyboardImage.SetActive(false);
        }

        if (mouseImage != null)
        {
            mouseImage.SetActive(true);
        }

        if (controlsNextButton != null)
        {
            controlsNextButton.SetActive(false);
        }
    }

    public void ToggleAudio()
    {
        audioEnabled = !audioEnabled;

        PlayerPrefs.SetInt(AudioKey, audioEnabled ? 1 : 0);
        PlayerPrefs.Save();

        ApplyAudioSetting();
    }

    public void ToggleFullscreen()
    {
        fullscreenEnabled = !fullscreenEnabled;

        PlayerPrefs.SetInt(
            FullscreenKey,
            fullscreenEnabled ? 1 : 0
        );
        PlayerPrefs.Save();

        ApplyFullscreenSetting();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void HandlePauseInput()
    {
        if (!isPaused)
        {
            OpenPause();
            return;
        }

        if (pauseSettingsPanel != null && pauseSettingsPanel.activeSelf)
        {
            ShowPauseMain();
            return;
        }

        if (pauseControlsPanel != null && pauseControlsPanel.activeSelf)
        {
            ShowPauseMain();
            return;
        }

        ResumeGame();
    }

    private void OpenPause()
    {
        isPaused = true;

        previousTimeScale = Time.timeScale;
        previousCursorLockMode = Cursor.lockState;
        previousCursorVisible = Cursor.visible;

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        ShowPauseMain();
    }

    private void ShowPauseMain()
    {
        SetPauseUIState(true, true, false, false);
    }

    private void ShowKeyboardControls()
    {
        if (keyboardImage != null)
        {
            keyboardImage.SetActive(true);
        }

        if (mouseImage != null)
        {
            mouseImage.SetActive(false);
        }

        if (controlsNextButton != null)
        {
            controlsNextButton.SetActive(true);
        }
    }

    private void HideAllPauseUI()
    {
        SetPauseUIState(false, false, false, false);
        ShowKeyboardControls();
    }

    private void SetPauseUIState(
        bool showDim,
        bool showPause,
        bool showSettings,
        bool showControls
    )
    {
        if (pauseDimBackground != null)
        {
            pauseDimBackground.SetActive(showDim);
        }

        if (pausePanel != null)
        {
            pausePanel.SetActive(showPause);
        }

        if (pauseSettingsPanel != null)
        {
            pauseSettingsPanel.SetActive(showSettings);
        }

        if (pauseControlsPanel != null)
        {
            pauseControlsPanel.SetActive(showControls);
        }
    }

    private void ApplyAudioSetting()
    {
        AudioListener.volume = audioEnabled ? 1f : 0f;
        UpdateSettingTexts();
    }

    private void ApplyFullscreenSetting()
    {
        Screen.fullScreen = fullscreenEnabled;
        UpdateSettingTexts();
    }

    private void UpdateSettingTexts()
    {
        if (audioButtonText != null)
        {
            audioButtonText.text = audioEnabled ? "ON" : "OFF";
        }

        if (fullscreenButtonText != null)
        {
            fullscreenButtonText.text = fullscreenEnabled ? "ON" : "OFF";
        }
    }
}