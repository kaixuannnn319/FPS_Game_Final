using System.Collections;
using TMPro;
using UnityEngine;

public class BuffStatusUIScript : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TMP_Text timerText;

    [Header("Temporary UI Test")]
    [SerializeField] private bool enableKeyboardPreview = true;
    [SerializeField] private float previewDuration = 15f;

    private Coroutine countdownCoroutine;

    private void Awake()
    {
        if (timerText == null)
        {
            Debug.LogError(
                "BuffStatusUIScript: BuffTimerText has not been assigned.",
                this
            );

            enabled = false;
            return;
        }

        // 游戏开始时，Buff 尚未使用，因此先隐藏倒计时。
        timerText.gameObject.SetActive(false);
    }

    private void Update()
    {
        // 临时测试：按键盘上方数字键 7，启动 15 秒倒计时。
        if (enableKeyboardPreview && Input.GetKeyDown(KeyCode.Alpha7))
        {
            StartBuffCountdown(previewDuration);
        }
    }

    /// <summary>
    /// 以后由正式的 Buff 系统调用。
    /// 这里只负责显示倒计时，不修改攻击力或 Buff 数据。
    /// </summary>
    public void StartBuffCountdown(float duration)
    {
        if (timerText == null)
        {
            return;
        }

        // 重复使用 Buff 时，重新从新的时间开始计算。
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }

        countdownCoroutine = StartCoroutine(Countdown(duration));
    }

    /// <summary>
    /// 提前结束 Buff 时，可由其他系统调用。
    /// </summary>
    public void StopBuffCountdown()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }

        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
        }
    }

    private IEnumerator Countdown(float duration)
    {
        timerText.gameObject.SetActive(true);

        float remainingTime = Mathf.Max(0f, duration);

        while (remainingTime > 0f)
        {
            int displayedSeconds = Mathf.CeilToInt(remainingTime);
            timerText.text = displayedSeconds + "s";

            remainingTime -= Time.deltaTime;
            yield return null;
        }

        timerText.text = "0s";

        // 让 0s 短暂显示一下。
        yield return new WaitForSeconds(0.2f);

        timerText.gameObject.SetActive(false);
        countdownCoroutine = null;
    }

    private void OnDisable()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
    }
}