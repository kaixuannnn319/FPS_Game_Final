using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitchUIController : MonoBehaviour
{
    [Header("Weapon Icon UI")]
    [SerializeField] private Image currentWeaponIcon;
    [SerializeField] private Image incomingWeaponIcon;

    [Header("Weapon Sprites")]
    [Tooltip("0 = Knife, 1 = Wand Level 1, 2 = Wand Level 2, 3 = Wand Level 3")]
    [SerializeField] private Sprite[] weaponIcons;

    [Header("Slide Animation")]
    [SerializeField] private float slideDistance = 120f;
    [SerializeField] private float animationDuration = 0.2f;

    [Header("Weapon Charge Bar")]
    [SerializeField] private GameObject weaponChargeBar;
    [SerializeField] private Slider weaponChargeSlider;
    [SerializeField] private Image chargeBarFill;

    [Header("Weapon Charge Bar Colours")]
    [SerializeField]
    private Color wandLevel1Colour =
        new Color32(53, 149, 211, 255);

    [SerializeField]
    private Color wandLevel2Colour =
        new Color32(74, 174, 88, 255);

    [SerializeField]
    private Color wandLevel3Colour =
        new Color32(217, 166, 46, 255);

    [Header("Weapon Ammo UI")]
    [Tooltip("拖入挂有 WeaponAmmoUIController 的 WeaponAmmoUI")]
    [SerializeField] private WeaponAmmoUIController weaponAmmoUI;

    [Header("Temporary Keyboard Test")]
    [Tooltip("测试阶段勾选，正式连接 Player 武器系统后取消勾选")]
    [SerializeField] private bool enableKeyboardTest = true;

    private RectTransform currentIconRect;
    private RectTransform incomingIconRect;

    private int currentWeaponIndex = 0;
    private bool isSwitching = false;

    private void Awake()
    {
        if (currentWeaponIcon == null || incomingWeaponIcon == null)
        {
            Debug.LogError(
                "WeaponSwitchUIController: Current or incoming weapon icon is missing."
            );

            enabled = false;
            return;
        }

        currentIconRect = currentWeaponIcon.rectTransform;
        incomingIconRect = incomingWeaponIcon.rectTransform;

        currentIconRect.anchoredPosition = Vector2.zero;
        incomingIconRect.anchoredPosition = Vector2.zero;

        incomingWeaponIcon.enabled = false;

        // 游戏开始时默认使用 Knife
        SetWeaponVisualImmediately(0);
    }

    private void Start()
    {
        /*
         * 放在 Start 中通知子弹 UI，
         * 可以确保 WeaponAmmoUIController 的 Awake 已经完成。
         */
        if (weaponAmmoUI != null)
        {
            weaponAmmoUI.SetWeapon(
                currentWeaponIndex,
                false
            );
        }
    }

    private void Update()
    {
        if (!enableKeyboardTest || isSwitching)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayWeaponSwitch(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayWeaponSwitch(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayWeaponSwitch(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayWeaponSwitch(3);
        }
    }

    /// <summary>
    /// 播放武器图标切换动画。
    ///
    /// 0 = Knife
    /// 1 = Wand Level 1
    /// 2 = Wand Level 2
    /// 3 = Wand Level 3
    /// </summary>
    public void PlayWeaponSwitch(int targetWeaponIndex)
    {
        if (isSwitching)
        {
            return;
        }

        if (!IsValidWeaponIndex(targetWeaponIndex))
        {
            Debug.LogWarning(
                "WeaponSwitchUIController: Invalid weapon index."
            );

            return;
        }

        // 重复选择当前武器时不播放动画
        if (targetWeaponIndex == currentWeaponIndex)
        {
            return;
        }

        StartCoroutine(
            SwitchAnimation(targetWeaponIndex)
        );
    }

    private IEnumerator SwitchAnimation(int targetWeaponIndex)
    {
        isSwitching = true;

        /*
         * 编号变大：
         * 当前图标向上离开，新图标从下方进入。
         *
         * 编号变小：
         * 当前图标向下离开，新图标从上方进入。
         */
        int direction =
            targetWeaponIndex > currentWeaponIndex ? 1 : -1;

        Vector2 currentStartPosition = Vector2.zero;

        Vector2 currentEndPosition =
            new Vector2(
                0f,
                slideDistance * direction
            );

        Vector2 incomingStartPosition =
            new Vector2(
                0f,
                -slideDistance * direction
            );

        Vector2 incomingEndPosition = Vector2.zero;

        incomingWeaponIcon.sprite =
            weaponIcons[targetWeaponIndex];

        incomingWeaponIcon.preserveAspect = true;
        incomingWeaponIcon.enabled = true;

        currentIconRect.anchoredPosition =
            currentStartPosition;

        incomingIconRect.anchoredPosition =
            incomingStartPosition;

        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(
                elapsedTime / animationDuration
            );

            float smoothProgress = Mathf.SmoothStep(
                0f,
                1f,
                progress
            );

            currentIconRect.anchoredPosition =
                Vector2.Lerp(
                    currentStartPosition,
                    currentEndPosition,
                    smoothProgress
                );

            incomingIconRect.anchoredPosition =
                Vector2.Lerp(
                    incomingStartPosition,
                    incomingEndPosition,
                    smoothProgress
                );

            yield return null;
        }

        currentWeaponIndex = targetWeaponIndex;

        currentWeaponIcon.sprite =
            weaponIcons[currentWeaponIndex];

        currentWeaponIcon.preserveAspect = true;

        currentIconRect.anchoredPosition = Vector2.zero;
        incomingIconRect.anchoredPosition = Vector2.zero;

        incomingWeaponIcon.enabled = false;

        // 更新能量条显示和颜色
        UpdateChargeBarAppearance(currentWeaponIndex);

        // 更新对应子弹槽
        if (weaponAmmoUI != null)
        {
            weaponAmmoUI.SetWeapon(
                currentWeaponIndex,
                true
            );
        }

        isSwitching = false;
    }

    /// <summary>
    /// 立即切换武器 UI，不播放动画。
    /// </summary>
    public void SetWeaponImmediately(int weaponIndex)
    {
        if (!IsValidWeaponIndex(weaponIndex))
        {
            Debug.LogWarning(
                "WeaponSwitchUIController: Invalid weapon index."
            );

            return;
        }

        StopAllCoroutines();
        isSwitching = false;

        SetWeaponVisualImmediately(weaponIndex);

        if (weaponAmmoUI != null)
        {
            weaponAmmoUI.SetWeapon(
                currentWeaponIndex,
                false
            );
        }
    }

    /// <summary>
    /// 立即设置图标和能量条，不通知子弹 UI。
    /// </summary>
    private void SetWeaponVisualImmediately(int weaponIndex)
    {
        currentWeaponIndex = weaponIndex;

        currentWeaponIcon.sprite =
            weaponIcons[currentWeaponIndex];

        currentWeaponIcon.preserveAspect = true;

        currentIconRect.anchoredPosition = Vector2.zero;
        incomingIconRect.anchoredPosition = Vector2.zero;

        incomingWeaponIcon.enabled = false;

        UpdateChargeBarAppearance(currentWeaponIndex);
    }

    /// <summary>
    /// 根据武器类型更新能量条。
    /// </summary>
    private void UpdateChargeBarAppearance(int weaponIndex)
    {
        if (weaponChargeBar == null)
        {
            return;
        }

        // Knife 不显示能量条
        if (weaponIndex == 0)
        {
            weaponChargeBar.SetActive(false);
            return;
        }

        // 法杖显示能量条
        weaponChargeBar.SetActive(true);

        if (chargeBarFill == null)
        {
            return;
        }

        switch (weaponIndex)
        {
            case 1:
                // Wand Level 1：蓝色
                chargeBarFill.color = wandLevel1Colour;
                break;

            case 2:
                // Wand Level 2：绿色
                chargeBarFill.color = wandLevel2Colour;
                break;

            case 3:
                // Wand Level 3：金色
                chargeBarFill.color = wandLevel3Colour;
                break;
        }
    }

    /// <summary>
    /// 使用 0 到 1 的比例更新能量条。
    /// </summary>
    public void SetChargeValue(float normalizedValue)
    {
        if (weaponChargeSlider == null)
        {
            return;
        }

        weaponChargeSlider.value =
            Mathf.Clamp01(normalizedValue);
    }

    /// <summary>
    /// 使用当前能量和最大能量更新能量条。
    /// </summary>
    public void SetChargeValue(
        float currentCharge,
        float maximumCharge
    )
    {
        if (weaponChargeSlider == null)
        {
            return;
        }

        if (maximumCharge <= 0f)
        {
            weaponChargeSlider.value = 0f;
            return;
        }

        weaponChargeSlider.value = Mathf.Clamp01(
            currentCharge / maximumCharge
        );
    }

    private bool IsValidWeaponIndex(int weaponIndex)
    {
        return weaponIcons != null &&
               weaponIndex >= 0 &&
               weaponIndex < weaponIcons.Length &&
               weaponIcons[weaponIndex] != null;
    }
}