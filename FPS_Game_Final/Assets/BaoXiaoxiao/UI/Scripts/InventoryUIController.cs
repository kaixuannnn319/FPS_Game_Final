using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
    private enum InventoryTab
    {
        Inventory,
        Equipment,
        Archive
    }

    [Header("UI References")]
    [SerializeField] private ItemSlotUI itemSlotPrefab;
    [SerializeField] private Transform itemGrid;
    [SerializeField] private TMP_Text itemListTitleText;
    [SerializeField] private GameObject itemListPanel;
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private GameObject archiveContent;

    [Header("Category Buttons")]
    [SerializeField] private Button inventoryTabButton;
    [SerializeField] private Button equipmentTabButton;
    [SerializeField] private Button archiveTabButton;

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

    [SerializeField] private InventoryController inventory;

    private InventoryTab currentTab = InventoryTab.Inventory;

    private void Awake()
    {
        if (inventory == null)
            inventory = FindFirstObjectByType<InventoryController>();

        if (inventoryTabButton != null)
        {
            inventoryTabButton.onClick.AddListener(ShowInventory);
        }

        if (equipmentTabButton != null)
        {
            equipmentTabButton.onClick.AddListener(ShowEquipment);
        }

        if (archiveTabButton != null)
        {
            archiveTabButton.onClick.AddListener(ShowArchive);
        }

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

        if (archiveTabButton != null)
        {
            archiveTabButton.onClick.RemoveListener(ShowArchive);
        }
    }

    public void ShowInventory()
    {
        currentTab = InventoryTab.Inventory;

        SetStandardContentVisible(true);
        SetArchiveVisible(false);

        ClearSlots();
        SetTitle("ITEMS");

        CreateItemSlot(medical1Icon, inventory.GetBandageCount());
        CreateItemSlot(medical2Icon, inventory.GetElixirCount());
        CreateItemSlot(buffIcon, inventory.GetBuffCount());

        CreateItemSlot(bullet1Icon, Mathf.RoundToInt(inventory.GetLevel1ReserveEnergy()));
        CreateItemSlot(bullet2Icon, Mathf.RoundToInt(inventory.GetLevel2ReserveEnergy()));
        CreateItemSlot(bullet3Icon, Mathf.RoundToInt(inventory.GetLevel3ReserveEnergy()));
    }

    public void ShowEquipment()
    {
        currentTab = InventoryTab.Equipment;

        SetStandardContentVisible(true);
        SetArchiveVisible(false);

        ClearSlots();
        SetTitle("EQUIPMENT");

        if (inventory.HasKnife())
        {
            CreateItemSlot(knifeIcon, 1);
        }

        if (inventory.HasLevel1Weapon())
            CreateItemSlot(wand1Icon, 1);

        if (inventory.HasLevel2Weapon())
            CreateItemSlot(wand2Icon, 1);

        if (inventory.HasLevel3Weapon())
            CreateItemSlot(wand3Icon, 1);
    }

    public void ShowArchive()
    {
        currentTab = InventoryTab.Archive;

        SetStandardContentVisible(false);
        SetArchiveVisible(true);
    }

    private void SetStandardContentVisible(bool visible)
    {
        if (itemListPanel != null)
        {
            itemListPanel.SetActive(visible);
        }

        if (detailPanel != null)
        {
            detailPanel.SetActive(visible);
        }
    }

    private void SetArchiveVisible(bool visible)
    {
        if (archiveContent != null)
        {
            archiveContent.SetActive(visible);
        }
    }

    private void CreateItemSlot(Sprite icon, int count)
    {
        if (icon == null || count <= 0)
        {
            return;
        }

        if (itemSlotPrefab == null || itemGrid == null)
        {
            Debug.LogError(
                "InventoryUIController: ItemSlotPrefab or ItemGrid is not assigned.",
                this
            );

            return;
        }

        ItemSlotUI newSlot = Instantiate(
            itemSlotPrefab,
            itemGrid
        );

        newSlot.name = $"ItemSlot_{icon.name}";
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

    private void OnEnable()
    {
        if (inventory != null)
        {
            inventory.OnInventoryChanged.AddListener(RefreshUI);
        }

        RefreshUI();
    }

    private void OnDisable()
    {
        if (inventory != null)
        {
            inventory.OnInventoryChanged.RemoveListener(RefreshUI);
        }
    }

    private void RefreshUI()
    {
        if (inventory == null)
            return;

        switch (currentTab)
        {
            case InventoryTab.Inventory:
                ShowInventory();
                break;

            case InventoryTab.Equipment:
                ShowEquipment();
                break;

            case InventoryTab.Archive:
                break;
        }
    }
}