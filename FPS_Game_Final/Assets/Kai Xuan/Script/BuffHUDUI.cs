using TMPro;
using UnityEngine;

public class BuffHUDUI : MonoBehaviour
{
    [SerializeField] private InventoryController inventory;

    [Header("UI")]
    [SerializeField] private GameObject buffIcon;
    [SerializeField] private GameObject buffInfo;
    [SerializeField] private TMP_Text buffCountText;
    [SerializeField] private TMP_Text buffKeyText;

    [SerializeField] private WeaponController weaponController;

    private bool unlocked = false;

    private void Start()
    {
        inventory.OnInventoryChanged.AddListener(UpdateUI);
        UpdateUI();
    }

    private void OnDestroy()
    {
        if (inventory != null)
            inventory.OnInventoryChanged.RemoveListener(UpdateUI);
    }

    private void UpdateUI()
    {
        int count = inventory.GetBuffCount();

        if (count > 0)
        {
            unlocked = true;
        }

        buffIcon.SetActive(unlocked);
        buffInfo.SetActive(unlocked);

        buffCountText.text = "x" + count;
        buffKeyText.text = "7";
    }
}