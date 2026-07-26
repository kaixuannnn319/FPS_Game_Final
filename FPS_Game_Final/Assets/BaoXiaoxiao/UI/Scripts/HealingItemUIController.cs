using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealingItemUIController : MonoBehaviour
{
    [Header("Healing Item Icons")]
    [SerializeField] private Image healingItem1Icon;
    [SerializeField] private Image healingItem2Icon;

    [Header("Use Feedback")]
    [SerializeField]
    private Color usedGreyColour =
        new Color32(100, 100, 100, 255);

    [SerializeField] private float greyDuration = 0.35f;

    [Header("Temporary Keyboard Test")]
    [SerializeField] private bool enableKeyboardTest = true;

    private Color healingItem1OriginalColour;
    private Color healingItem2OriginalColour;

    private Coroutine healingItem1Coroutine;
    private Coroutine healingItem2Coroutine;

    private void Awake()
    {
        if (healingItem1Icon == null || healingItem2Icon == null)
        {
            Debug.LogError(
                "HealingItemUIController: Healing item icon references are missing."
            );

            enabled = false;
            return;
        }

        healingItem1OriginalColour = healingItem1Icon.color;
        healingItem2OriginalColour = healingItem2Icon.color;
    }

    private void Update()
    {
        if (!enableKeyboardTest)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PlayHealingItem1Feedback();
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            PlayHealingItem2Feedback();
        }
    }

    public void PlayHealingItem1Feedback()
    {
        if (healingItem1Coroutine != null)
        {
            StopCoroutine(healingItem1Coroutine);
        }

        healingItem1Coroutine = StartCoroutine(
            PlayGreyFeedback(
                healingItem1Icon,
                healingItem1OriginalColour,
                0
            )
        );
    }

    public void PlayHealingItem2Feedback()
    {
        if (healingItem2Coroutine != null)
        {
            StopCoroutine(healingItem2Coroutine);
        }

        healingItem2Coroutine = StartCoroutine(
            PlayGreyFeedback(
                healingItem2Icon,
                healingItem2OriginalColour,
                1
            )
        );
    }

    public void PlayHealingItemFeedback(int itemIndex)
    {
        switch (itemIndex)
        {
            case 0:
                PlayHealingItem1Feedback();
                break;

            case 1:
                PlayHealingItem2Feedback();
                break;

            default:
                Debug.LogWarning(
                    "HealingItemUIController: Invalid healing item index."
                );
                break;
        }
    }

    private IEnumerator PlayGreyFeedback(
        Image targetIcon,
        Color originalColour,
        int itemIndex
    )
    {
        targetIcon.color = usedGreyColour;

        yield return new WaitForSecondsRealtime(
            greyDuration
        );

        targetIcon.color = originalColour;

        if (itemIndex == 0)
        {
            healingItem1Coroutine = null;
        }
        else
        {
            healingItem2Coroutine = null;
        }
    }
}