using UnityEngine;

public class InventoryToggleUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private KeyCode toggleKey = KeyCode.Tab;
    [SerializeField] private bool pauseGameWhileOpen = true;

    private bool isOpen;
    public bool IsOpen => isOpen;
    private float previousTimeScale = 1f;
    private CursorLockMode previousCursorLockMode;
    private bool previousCursorVisible;

    private float ignoreInputUntil;
    public bool IgnoreGameplayInput => Time.unscaledTime < ignoreInputUntil;


    private void Awake()
    {
        if (inventoryPanel == null)
        {
            Debug.LogError(
                "InventoryToggleUI: Inventory Panel is not assigned.",
                this
            );

            return;
        }

        inventoryPanel.SetActive(false);
        isOpen = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (pauseGameWhileOpen)
        {
            Time.timeScale = 1f;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        SetInventoryOpen(!isOpen);
    }

    public void OpenInventory()
    {
        SetInventoryOpen(true);
    }

    public void CloseInventory()
    {
        SetInventoryOpen(false);
    }

    private void SetInventoryOpen(bool open)
    {
        if (inventoryPanel == null || isOpen == open)
        {
            return;
        }

        isOpen = open;

        if (open)
        {
            previousTimeScale = Time.timeScale;
            previousCursorLockMode = Cursor.lockState;
            previousCursorVisible = Cursor.visible;

            inventoryPanel.SetActive(true);

            if (pauseGameWhileOpen)
            {
                Time.timeScale = 0f;
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            inventoryPanel.SetActive(false);

            if (pauseGameWhileOpen)
            {
                Time.timeScale = previousTimeScale;
            }

            Cursor.lockState = previousCursorLockMode;
            Cursor.visible = previousCursorVisible;

            ignoreInputUntil = Time.unscaledTime + 0.15f;
        }
    }

    private void OnDestroy()
    {
        if (!isOpen)
        {
            return;
        }

        if (pauseGameWhileOpen)
        {
            Time.timeScale = previousTimeScale;
        }

        Cursor.lockState = previousCursorLockMode;
        Cursor.visible = previousCursorVisible;
    }
}