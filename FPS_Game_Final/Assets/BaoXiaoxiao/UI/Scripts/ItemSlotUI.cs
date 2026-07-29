using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text countText;

    /// <summary>
    /// 显示一个物品。
    /// </summary>
    public void Setup(Sprite icon, int count)
    {
        if (itemIcon == null || countText == null)
        {
            Debug.LogError("ItemSlotUI: ItemIcon or CountText is not assigned.", this);
            return;
        }

        // 设置并显示物品图片
        itemIcon.sprite = icon;
        itemIcon.gameObject.SetActive(icon != null);

        // 数量大于 1 时才显示数字
        bool shouldShowCount = count > 1;

        countText.text = shouldShowCount
            ? count.ToString()
            : string.Empty;

        countText.gameObject.SetActive(shouldShowCount);
    }

    /// <summary>
    /// 清空格子。
    /// </summary>
    public void ClearSlot()
    {
        if (itemIcon != null)
        {
            itemIcon.sprite = null;
            itemIcon.gameObject.SetActive(false);
        }

        if (countText != null)
        {
            countText.text = string.Empty;
            countText.gameObject.SetActive(false);
        }
    }
}