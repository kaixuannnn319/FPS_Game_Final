using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WeaponAmmoUIController : MonoBehaviour
{
    [Header("Ammo Slot Objects")]
    [Tooltip("顺序必须是 Basic、Enhanced、Sacred")]
    [SerializeField] private RectTransform[] ammoSlots;

    [Header("Ammo Icons")]
    [Tooltip("顺序必须是 Basic、Enhanced、Sacred")]
    [SerializeField] private Image[] ammoIcons;

    [Header("Selected Frames")]
    [Tooltip("顺序必须是 Basic、Enhanced、Sacred")]
    [SerializeField] private GameObject[] selectedFrames;

    [Header("Selection Animation")]
    [SerializeField] private float selectedMoveDistance = 10f;
    [SerializeField] private float moveDuration = 0.15f;

    [Header("Reload Visual Test")]
    [SerializeField] private bool enableReloadKeyboardTest = true;
    [SerializeField]
    private Color reloadGreyColour =
        new Color32(110, 110, 110, 255);

    [SerializeField] private float reloadGreyDuration = 0.4f;

    private Vector2[] originalPositions;
    private Color[] originalIconColours;

    private int selectedAmmoIndex = -1;

    private Coroutine moveCoroutine;
    private Coroutine reloadCoroutine;

    private void Awake()
    {
        if (!ReferencesAreValid())
        {
            Debug.LogError(
                "WeaponAmmoUIController: Please assign 3 slots, 3 icons and 3 selected frames."
            );

            enabled = false;
            return;
        }

        originalPositions = new Vector2[ammoSlots.Length];
        originalIconColours = new Color[ammoIcons.Length];

        for (int i = 0; i < ammoSlots.Length; i++)
        {
            originalPositions[i] = ammoSlots[i].anchoredPosition;
            originalIconColours[i] = ammoIcons[i].color;
        }

        // 游戏开始默认是 Knife，所以不选择任何子弹
        SetWeapon(0, false);
    }

    private void Update()
    {
        if (!enableReloadKeyboardTest)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayReloadFeedback();
        }
    }

    /// <summary>
    /// 根据武器编号选择对应的子弹。
    /// 0 = Knife
    /// 1 = Wand Level 1 / Basic
    /// 2 = Wand Level 2 / Enhanced
    /// 3 = Wand Level 3 / Sacred
    /// </summary>
    public void SetWeapon(int weaponIndex, bool animate = true)
    {
        switch (weaponIndex)
        {
            case 0:
                SelectAmmoSlot(-1, false);
                break;

            case 1:
                SelectAmmoSlot(0, animate);
                break;

            case 2:
                SelectAmmoSlot(1, animate);
                break;

            case 3:
                SelectAmmoSlot(2, animate);
                break;

            default:
                Debug.LogWarning(
                    "WeaponAmmoUIController: Invalid weapon index."
                );
                break;
        }
    }

    /// <summary>
    /// 选择一个子弹槽。
    /// -1 表示没有选中任何子弹。
    /// </summary>
    public void SelectAmmoSlot(int ammoIndex, bool animate = true)
    {
        if (ammoIndex < -1 || ammoIndex >= ammoSlots.Length)
        {
            return;
        }

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }

        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
            reloadCoroutine = null;
        }

        // 所有图标恢复原来的颜色和位置
        for (int i = 0; i < ammoSlots.Length; i++)
        {
            ammoSlots[i].anchoredPosition = originalPositions[i];
            ammoIcons[i].color = originalIconColours[i];
            selectedFrames[i].SetActive(false);
        }

        selectedAmmoIndex = ammoIndex;

        // Knife：不选择任何子弹
        if (selectedAmmoIndex == -1)
        {
            return;
        }

        selectedFrames[selectedAmmoIndex].SetActive(true);

        Vector2 targetPosition =
            originalPositions[selectedAmmoIndex] +
            Vector2.up * selectedMoveDistance;

        if (animate)
        {
            moveCoroutine = StartCoroutine(
                MoveSelectedSlot(
                    selectedAmmoIndex,
                    targetPosition
                )
            );
        }
        else
        {
            ammoSlots[selectedAmmoIndex].anchoredPosition =
                targetPosition;
        }
    }

    private IEnumerator MoveSelectedSlot(
        int ammoIndex,
        Vector2 targetPosition
    )
    {
        Vector2 startPosition =
            originalPositions[ammoIndex];

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(
                elapsedTime / moveDuration
            );

            float smoothProgress = Mathf.SmoothStep(
                0f,
                1f,
                progress
            );

            ammoSlots[ammoIndex].anchoredPosition =
                Vector2.Lerp(
                    startPosition,
                    targetPosition,
                    smoothProgress
                );

            yield return null;
        }

        ammoSlots[ammoIndex].anchoredPosition =
            targetPosition;

        moveCoroutine = null;
    }

    /// <summary>
    /// 当前选中的子弹图标短暂变灰。
    /// </summary>
    public void PlayReloadFeedback()
    {
        // Knife 状态下没有子弹，不执行
        if (selectedAmmoIndex < 0)
        {
            return;
        }

        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
        }

        reloadCoroutine = StartCoroutine(
            ReloadGreyAnimation(selectedAmmoIndex)
        );
    }

    private IEnumerator ReloadGreyAnimation(int ammoIndex)
    {
        ammoIcons[ammoIndex].color = reloadGreyColour;

        yield return new WaitForSecondsRealtime(
            reloadGreyDuration
        );

        ammoIcons[ammoIndex].color =
            originalIconColours[ammoIndex];

        reloadCoroutine = null;
    }

    private bool ReferencesAreValid()
    {
        return ammoSlots != null &&
               ammoIcons != null &&
               selectedFrames != null &&
               ammoSlots.Length == 3 &&
               ammoIcons.Length == 3 &&
               selectedFrames.Length == 3;
    }
}