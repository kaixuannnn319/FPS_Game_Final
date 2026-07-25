using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private ItemSlotUI itemSlotPrefab;
    [SerializeField] private Transform itemGrid;
    [SerializeField] private TMP_Text itemListTitleText;

    [Header("Category Buttons")]
    [SerializeField] private Button inventoryTabButton;
    [SerializeField] private Button equipmentTabButton;

    [Header("Inventory Icons")]
    [SerializeField] private Sprite medical1Icon;
    [SerializeField] private Sprite medical2Icon;
    [SerializeField] private Sprite buffIcon;
    [SerializeField] private Sprite bullet1Icon;
    [SerializeField] private Sprite bullet2Icon;
    [SerializeField] private Sprite bullet3Icon;

    [Header("Equipment Icons")]
    [SerializeField] private Sprite knifeIcon;
    [SerializeField] private Sprite wand1Icon;
    [SerializeField] private Sprite wand2Icon;
    [SerializeField] private Sprite wand3Icon;

    [Header("Temporary Inventory Counts")]
    [SerializeField] private int medical1Count = 3;
    [SerializeField] private int medical2Count = 1;
    [SerializeField] private int buffCount = 1;
    [SerializeField] private int bullet1Count = 20;
    [SerializeField] private int bullet2Count = 10;
    [SerializeField] private int bullet3Count = 5;

    [Header("Temporary Owned Equipment")]
    [SerializeField] private bool hasKnife = true;
    [SerializeField] private bool hasWand1 = true;
    [SerializeField] private bool hasWand2 = true;
    [SerializeField] private bool hasWand3 = true;

    private void Awake()
    {
        if (inventoryTabButton != null)
        {
            inventoryTabButton.onClick.AddListener(ShowInventory);
        }

        if (equipmentTabButton != null)
        {
            equipmentTabButton.onClick.AddListener(ShowEquipment);
        }

        // 打开背包时，默认显示 Inventory 页面
        ShowInventory();
    }

    private void OnDestroy()
    {
        if (inventoryTabButton != null)
        {
            inventoryTabButton.onClick.RemoveListener(ShowInventory);
        }

        if (equipmentTabButton != null)
        {
            equipmentTabButton.onClick.RemoveListener(ShowEquipment);
        }
    }

    public void ShowInventory()
    {
        ClearSlots();
        SetTitle("ITEMS");

        CreateItemSlot(medical1Icon, medical1Count);
        CreateItemSlot(medical2Icon, medical2Count);
        CreateItemSlot(buffIcon, buffCount);

        CreateItemSlot(bullet1Icon, bullet1Count);
        CreateItemSlot(bullet2Icon, bullet2Count);
        CreateItemSlot(bullet3Icon, bullet3Count);
    }

    public void ShowEquipment()
    {
        ClearSlots();
        SetTitle("EQUIPMENT");

        // 目前用勾选框模拟玩家是否已经收集武器
        if (hasKnife)
        {
            CreateItemSlot(knifeIcon, 1);
        }

        if (hasWand1)
        {
            CreateItemSlot(wand1Icon, 1);
        }

        if (hasWand2)
        {
            CreateItemSlot(wand2Icon, 1);
        }

        if (hasWand3)
        {
            CreateItemSlot(wand3Icon, 1);
        }
    }

    private void CreateItemSlot(Sprite icon, int count)
    {
        // 数量为 0 或没有图片时，不生成格子
        if (icon == null || count <= 0)
        {
            return;
        }

        if (itemSlotPrefab == null || itemGrid == null)
        {
            Debug.LogError(
                "InventoryUIController: ItemSlot Prefab or ItemGrid is not assigned.",
                this
            );

            return;
        }

        ItemSlotUI newSlot = Instantiate(itemSlotPrefab, itemGrid);
        newSlot.name = "ItemSlot_" + icon.name;
        newSlot.Setup(icon, count);
    }

    private void ClearSlots()
    {
        if (itemGrid == null)
        {
            return;
        }

        for (int i = itemGrid.childCount - 1; i >= 0; i--)
        {
            Destroy(itemGrid.GetChild(i).gameObject);
        }
    }

    private void SetTitle(string title)
    {
        if (itemListTitleText != null)
        {
            itemListTitleText.text = title;
        }
    }
}