using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitchUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image currentWeaponIcon;
    [SerializeField] private Image incomingWeaponIcon;

    [Header("Weapon Icons")]
    [SerializeField] private Sprite[] weaponSprites;

    [Header("Slide Animation")]
    [SerializeField] private float slideDistance = 150f;
    [SerializeField] private float slideDuration = 0.2f;

    [Header("UI Scene Testing")]
    [Tooltip("Only enable this in the UI test scene. Disable it after connecting the real weapon system.")]
    [SerializeField] private bool enableMouseWheelPreview = true;

    private RectTransform currentRect;
    private RectTransform incomingRect;

    private Vector2 centrePosition;
    private int currentWeaponIndex;
    private bool isAnimating;

    private void Awake()
    {
        if (currentWeaponIcon == null || incomingWeaponIcon == null)
        {
            Debug.LogError(
                "WeaponSwitchUI: CurrentWeaponIcon or IncomingWeaponIcon is not assigned.",
                this
            );
            enabled = false;
            return;
        }

        currentRect = currentWeaponIcon.rectTransform;
        incomingRect = incomingWeaponIcon.rectTransform;

        // Records your existing position: X = -74.5, Y = -72.7.
        centrePosition = currentRect.anchoredPosition;

        currentRect.anchoredPosition = centrePosition;
        incomingRect.anchoredPosition =
            centrePosition + Vector2.down * slideDistance;

        if (weaponSprites != null && weaponSprites.Length > 0)
        {
            currentWeaponIndex = 0;
            currentWeaponIcon.sprite = weaponSprites[0];
        }
    }

    private void Update()
    {
        if (!enableMouseWheelPreview || isAnimating)
        {
            return;
        }

        float scrollValue = Input.mouseScrollDelta.y;

        if (scrollValue > 0f)
        {
            PreviewSwitch(1);
        }
        else if (scrollValue < 0f)
        {
            PreviewSwitch(-1);
        }
    }

    private void PreviewSwitch(int direction)
    {
        if (weaponSprites == null || weaponSprites.Length < 2)
        {
            return;
        }

        int nextIndex =
            (currentWeaponIndex + direction + weaponSprites.Length)
            % weaponSprites.Length;

        ShowWeapon(nextIndex, direction);
    }

    /// <summary>
    /// Called by the real weapon system later.
    /// direction: 1 = slide upward, -1 = slide downward.
    /// </summary>
    public void ShowWeapon(int weaponIndex, int direction)
    {
        if (weaponSprites == null ||
            weaponIndex < 0 ||
            weaponIndex >= weaponSprites.Length ||
            isAnimating ||
            weaponIndex == currentWeaponIndex)
        {
            return;
        }

        StartCoroutine(SlideWeapon(weaponIndex, direction));
    }

    public void ShowWeaponImmediately(int weaponIndex)
    {
        if (weaponSprites == null ||
            weaponIndex < 0 ||
            weaponIndex >= weaponSprites.Length)
        {
            return;
        }

        StopAllCoroutines();
        isAnimating = false;

        currentWeaponIndex = weaponIndex;
        currentWeaponIcon.sprite = weaponSprites[weaponIndex];

        currentRect.anchoredPosition = centrePosition;
        incomingRect.anchoredPosition =
            centrePosition + Vector2.down * slideDistance;
    }

    private IEnumerator SlideWeapon(int nextIndex, int direction)
    {
        isAnimating = true;

        float movementDirection = direction >= 0 ? 1f : -1f;

        Vector2 currentStart = centrePosition;
        Vector2 currentEnd =
            centrePosition + Vector2.up * slideDistance * movementDirection;

        Vector2 incomingStart =
            centrePosition - Vector2.up * slideDistance * movementDirection;
        Vector2 incomingEnd = centrePosition;

        incomingWeaponIcon.sprite = weaponSprites[nextIndex];

        currentRect.anchoredPosition = currentStart;
        incomingRect.anchoredPosition = incomingStart;

        float elapsedTime = 0f;

        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsedTime / slideDuration);
            float smoothProgress =
                progress * progress * (3f - 2f * progress);

            currentRect.anchoredPosition =
                Vector2.Lerp(currentStart, currentEnd, smoothProgress);

            incomingRect.anchoredPosition =
                Vector2.Lerp(incomingStart, incomingEnd, smoothProgress);

            yield return null;
        }

        currentWeaponIndex = nextIndex;
        currentWeaponIcon.sprite = weaponSprites[nextIndex];

        currentRect.anchoredPosition = centrePosition;
        incomingRect.anchoredPosition = incomingStart;

        isAnimating = false;
    }
}
