using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject controlsPanel;

    [Header("Settings Text")]
    [SerializeField] private TMP_Text audioButtonText;
    [SerializeField] private TMP_Text fullscreenButtonText;

    [Header("Scene")]
    [SerializeField] private string firstLevelSceneName = "Level 1";

    private const string AudioKey = "AudioEnabled";
    private const string FullscreenKey = "FullscreenEnabled";

    private bool audioEnabled;
    private bool fullscreenEnabled;

    private void Start()
    {
        audioEnabled = PlayerPrefs.GetInt(AudioKey, 1) == 1;
        fullscreenEnabled = PlayerPrefs.GetInt(
            FullscreenKey,
            Screen.fullScreen ? 1 : 0
        ) == 1;

        ApplyAudioSetting();
        ApplyFullscreenSetting();
        ShowMainMenu();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(firstLevelSceneName);
    }

    public void OpenSettings()
    {
        SetPanelState(false, true, false);
        UpdateSettingTexts();
    }

    public void CloseSettings()
    {
        ShowMainMenu();
    }

    public void OpenControls()
    {
        SetPanelState(false, false, true);
    }

    public void CloseControls()
    {
        ShowMainMenu();
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

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    private void ShowMainMenu()
    {
        SetPanelState(true, false, false);
    }

    private void SetPanelState(
        bool showMainMenu,
        bool showSettings,
        bool showControls
    )
    {
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(showMainMenu);
        }

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(showSettings);
        }

        if (controlsPanel != null)
        {
            controlsPanel.SetActive(showControls);
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
            fullscreenButtonText.text =
                fullscreenEnabled ? "ON" : "OFF";
        }
    }
}