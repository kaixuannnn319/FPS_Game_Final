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

    private int bandageCount;
    private int elixirCount;
    private int buffCount;

    private float reserveEnergy1;
    private float reserveEnergy2;
    private float reserveEnergy3;

    private bool hasKnife;
    private bool hasWand1;
    private bool hasWand2;
    private bool hasWand3;

    private bool showingInventory = true;

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
        showingInventory = true;

        ClearSlots();
        SetTitle("ITEMS");

        CreateItemSlot(medical1Icon, bandageCount);
        CreateItemSlot(medical2Icon, elixirCount);
        CreateItemSlot(buffIcon, buffCount);

        CreateItemSlot(bullet1Icon, Mathf.RoundToInt(reserveEnergy1));
        CreateItemSlot(bullet2Icon, Mathf.RoundToInt(reserveEnergy2));
        CreateItemSlot(bullet3Icon, Mathf.RoundToInt(reserveEnergy3));
    }

    public void ShowEquipment()
    {
        showingInventory = false;

        ClearSlots();
        SetTitle("EQUIPMENT");

        if (hasKnife)
            CreateItemSlot(knifeIcon, 1);

        if (hasWand1)
            CreateItemSlot(wand1Icon, 1);

        if (hasWand2)
            CreateItemSlot(wand2Icon, 1);

        if (hasWand3)
            CreateItemSlot(wand3Icon, 1);
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

    public void UpdateInventory(
    int bandages,
    int elixirs,
    int buffs,
    float reserve1,
    float reserve2,
    float reserve3,
    bool knife,
    bool wand1,
    bool wand2,
    bool wand3)
    {
        bandageCount = bandages;
        elixirCount = elixirs;
        buffCount = buffs;

        reserveEnergy1 = reserve1;
        reserveEnergy2 = reserve2;
        reserveEnergy3 = reserve3;

        hasKnife = knife;
        hasWand1 = wand1;
        hasWand2 = wand2;
        hasWand3 = wand3;

        // Refresh whichever tab is currently open
        if (showingInventory)
        {
            ShowInventory();
        }
        else
        {
            ShowEquipment();
        }
    }
}