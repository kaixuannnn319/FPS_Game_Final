using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
public enum WeaponType
{
    None,
    Knife,
    WandLevel1,
    WandLevel2,
    WandLevel3
}

[System.Serializable]
public class InventorySnapshot
{
    public float level1Energy;
    public float level2Energy;
    public float level3Energy;

    public float level1Reserve;
    public float level2Reserve;
    public float level3Reserve;

    public int bandages;
    public int elixirs;
    public int buffs;
}

public class InventoryController : MonoBehaviour
{
    [Header("Energy Pickup Amount")]
    [SerializeField] private float level1PickupAmount = 5f;
    [SerializeField] private float level2PickupAmount = 10f;
    [SerializeField] private float level3PickupAmount = 20f;

    [Header("Magic Energy")]
    // Current
    [SerializeField] private float level1Energy = 100f;
    [SerializeField] private float level2Energy = 100f;
    [SerializeField] private float level3Energy = 100f;
    // Reserve
    [SerializeField] private float level1ReserveEnergy = 0f;
    [SerializeField] private float level2ReserveEnergy = 0f;
    [SerializeField] private float level3ReserveEnergy = 0f;
    // Max Reserve Energy
    [SerializeField] private float maxLevel1ReserveEnergy = 100f;
    [SerializeField] private float maxLevel2ReserveEnergy = 200f;
    [SerializeField] private float maxLevel3ReserveEnergy = 200f;


    [Header("Healing Items")]
    [SerializeField] private int bandageCount = 0;
    [SerializeField] private int elixirCount = 0;
    [SerializeField] private int maxBandage = 3;
    [SerializeField] private int maxElixir = 1;

    [Header("Weapon Unlock")]
    [SerializeField] private bool hasKnife = false;
    [SerializeField] private bool hasWandLevel1 = false;
    [SerializeField] private bool hasWandLevel2 = false;
    [SerializeField] private bool hasWandLevel3 = false;
    [SerializeField] private WeaponType currentWeaponType = WeaponType.None;


    [Header("Quest Items")]
    private List<string> collectedKeys = new List<string>();
    [SerializeField] private bool hasKey1 = false;
    [SerializeField] private bool hasKey2 = false;
    [SerializeField] private bool hasKey3 = false;

    [SerializeField] private int sealCount = 0;
    [SerializeField] private int buffCount = 0;
    [SerializeField] private int maxBuff = 3;

    public UnityEvent OnInventoryChanged = new UnityEvent();

    private InventorySnapshot checkpointSnapshot;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadInventory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetBuffCount()
    {
        return buffCount;
    }

    public WeaponType GetCurrentWeaponType()
    {
        return currentWeaponType;
    }

    public float GetLevel1Energy()
    {
        return level1Energy;
    }
    public float GetLevel2Energy()
    {
        return level2Energy;
    }
    public float GetLevel3Energy()
    {
        return level3Energy;
    }
    public float GetLevel1ReserveEnergy()
    {
        return level1ReserveEnergy;
    }

    public float GetLevel2ReserveEnergy()
    {
        return level2ReserveEnergy;
    }

    public float GetLevel3ReserveEnergy()
    {
        return level3ReserveEnergy;
    }
    public int GetBandageCount()
    {
        return bandageCount;
    }
    public int GetElixirCount()
    {
        return elixirCount;
    }
    public bool HasKnife()
    {
        return hasKnife;
    }

    public bool HasLevel1Weapon()
    {
        return hasWandLevel1;
    }

    public bool HasLevel2Weapon()
    {
        return hasWandLevel2;
    }

    public bool HasLevel3Weapon()
    {
        return hasWandLevel3;
    }

    public int GetMaxBandage()
    {
        return maxBandage;
    }

    public int GetMaxElixir()
    {
        return maxElixir;
    }

    public int GetRelicCount()
    {
        return sealCount;
    }

    public int GetMaxBuff()
    {
        return maxBuff;
    }
    public void UnlockWeapon(WeaponType weapon)
    {
        switch (weapon)
        {
            case WeaponType.Knife:
                hasKnife = true;
                break;

            case WeaponType.WandLevel1:
                hasWandLevel1 = true;
                break;

            case WeaponType.WandLevel2:
                hasWandLevel2 = true;
                break;

            case WeaponType.WandLevel3:
                hasWandLevel3 = true;
                break;
        }

        OnInventoryChanged?.Invoke();

        SaveInventory();

        Debug.Log("Unlocked : " + weapon);
    }



    public bool SwitchWeapon(WeaponType weapon)
    {
        switch (weapon)
        {
            case WeaponType.Knife:
                if (!hasKnife) return false;
                break;

            case WeaponType.WandLevel1:
                if (!hasWandLevel1) return false;
                break;

            case WeaponType.WandLevel2:
                if (!hasWandLevel2) return false;
                break;

            case WeaponType.WandLevel3:
                if (!hasWandLevel3) return false;
                break;
        }

        currentWeaponType = weapon;
        Debug.Log("Current Weapon : " + currentWeaponType);
        return true;
    }

    public bool AddLevel1ReserveEnergy()
    {
        if (level1ReserveEnergy >= maxLevel1ReserveEnergy)
            return false;

        level1ReserveEnergy += level1PickupAmount;

        level1ReserveEnergy = Mathf.Min(level1ReserveEnergy, maxLevel1ReserveEnergy);

        OnInventoryChanged?.Invoke();

        return true;
    }
    public bool AddLevel2ReserveEnergy()
    {
        if (level2ReserveEnergy >= maxLevel2ReserveEnergy)
            return false;

        level2ReserveEnergy += level2PickupAmount;

        level2ReserveEnergy = Mathf.Min(level2ReserveEnergy, maxLevel2ReserveEnergy);

        OnInventoryChanged?.Invoke();

        return true;
    }

    public bool AddLevel3ReserveEnergy()
    {
        if (level3ReserveEnergy >= maxLevel3ReserveEnergy)
            return false;

        level3ReserveEnergy += level3PickupAmount;

        level3ReserveEnergy = Mathf.Min(level3ReserveEnergy, maxLevel3ReserveEnergy);

        OnInventoryChanged?.Invoke();

        return true;
    }

    public bool AddBandage()
    {
        if (bandageCount >= maxBandage)
            return false;

        bandageCount++;

        OnInventoryChanged?.Invoke();

        return true;
    }

    public bool UseBandage()
    {
        if (bandageCount <= 0)
            return false;

        bandageCount--;

        OnInventoryChanged?.Invoke();

        Debug.Log("Bandage Left : " + bandageCount);

        return true;
    }

    public bool AddElixir()
    {
        if (elixirCount >= maxElixir)
            return false;

        elixirCount++;

        OnInventoryChanged?.Invoke();

        return true;
    }

    public bool UseElixir()
    {
        if (elixirCount <= 0)
            return false;

        elixirCount--;

        OnInventoryChanged?.Invoke();

        Debug.Log("Elixir Left : " + elixirCount);
        return true;
    }
    public bool AddBuff()
    {
        if (buffCount >= maxBuff)
            return false;

        buffCount++;

        OnInventoryChanged?.Invoke();

        return true;
    }

    public bool UseBuff()
    {
        if (buffCount <= 0)
            return false;

        buffCount--;

        OnInventoryChanged?.Invoke();

        Debug.Log("Buff Left : " + buffCount);

        return true;
    }

    public void AddRelic()
    {
        sealCount++;
        OnInventoryChanged?.Invoke();

        SaveInventory();
    }

    public void CollectKey(string keyID)
    {
        if (collectedKeys.Contains(keyID))
            return;

        collectedKeys.Add(keyID);

        switch (keyID)
        {
            case "Key1":
                hasKey1 = true;
                break;

            case "Key2":
                hasKey2 = true;
                break;

            case "Key3":
                hasKey3 = true;
                break;
        }

        Debug.Log("Collected Key : " + keyID);

        SaveInventory();
    }

    public bool HasKey(string keyID)
    {
        return collectedKeys.Contains(keyID);
    }
    public bool UseEnergy(WeaponType weaponType, float amount)
    {
        switch (weaponType)
        {
            case WeaponType.WandLevel1:

                if (level1Energy < amount)
                    return false;

                level1Energy -= amount;
                OnInventoryChanged?.Invoke();
                return true;

            case WeaponType.WandLevel2:

                if (level2Energy < amount)
                    return false;

                level2Energy -= amount;
                OnInventoryChanged?.Invoke();
                return true;

            case WeaponType.WandLevel3:

                if (level3Energy < amount)
                    return false;

                level3Energy -= amount;
                OnInventoryChanged?.Invoke();
                return true;
        }

        return false;
    }
    public bool ReloadEnergy(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.WandLevel1:

                if (level1ReserveEnergy <= 0)
                    return false;

                float need1 = 100f - level1Energy;
                float give1 = Mathf.Min(need1, level1ReserveEnergy);

                level1Energy += give1;
                level1ReserveEnergy -= give1;

                OnInventoryChanged?.Invoke();

                Debug.Log($"Level1 Reload : {level1Energy}/100 | Reserve : {level1ReserveEnergy}");
                return true;

            case WeaponType.WandLevel2:

                if (level2ReserveEnergy <= 0)
                    return false;

                float need2 = 100f - level2Energy;
                float give2 = Mathf.Min(need2, level2ReserveEnergy);

                level2Energy += give2;
                level2ReserveEnergy -= give2;

                OnInventoryChanged?.Invoke();

                Debug.Log($"Level2 Reload : {level2Energy}/100 | Reserve : {level2ReserveEnergy}");
                return true;

            case WeaponType.WandLevel3:

                if (level3ReserveEnergy <= 0)
                    return false;

                float need3 = 100f - level3Energy;
                float give3 = Mathf.Min(need3, level3ReserveEnergy);

                level3Energy += give3;
                level3ReserveEnergy -= give3;

                OnInventoryChanged?.Invoke();

                Debug.Log($"Level3 Reload : {level3Energy}/100 | Reserve : {level3ReserveEnergy}");
                return true;
        }

        return false;
    }
    public float TakeReserveEnergy(WeaponType weaponType, float amount)
    {
        switch (weaponType)
        {
            case WeaponType.WandLevel1:

                float give1 = Mathf.Min(amount, level1ReserveEnergy);
                level1ReserveEnergy -= give1;
                OnInventoryChanged?.Invoke();
                return give1;

            case WeaponType.WandLevel2:

                float give2 = Mathf.Min(amount, level2ReserveEnergy);
                level2ReserveEnergy -= give2;
                OnInventoryChanged?.Invoke();
                return give2;

            case WeaponType.WandLevel3:

                float give3 = Mathf.Min(amount, level3ReserveEnergy);
                level3ReserveEnergy -= give3;
                OnInventoryChanged?.Invoke();
                return give3;
        }

        return 0;
    }

    public void SaveInventory()
    {
        PlayerData.hasKnife = HasKnife();
        PlayerData.hasWand1 = HasLevel1Weapon();
        PlayerData.hasWand2 = HasLevel2Weapon();
        PlayerData.hasWand3 = HasLevel3Weapon();

        PlayerData.currentWeapon = GetCurrentWeaponType();

        PlayerData.level1Energy = GetLevel1Energy();
        PlayerData.level2Energy = GetLevel2Energy();
        PlayerData.level3Energy = GetLevel3Energy();

        PlayerData.level1Reserve = GetLevel1ReserveEnergy();
        PlayerData.level2Reserve = GetLevel2ReserveEnergy();
        PlayerData.level3Reserve = GetLevel3ReserveEnergy();

        PlayerData.hasSave = true;
    }

    public void LoadInventory()
    {
        if (!PlayerData.hasSave)
            return;

        if (PlayerData.hasKnife)
            UnlockWeapon(WeaponType.Knife);

        if (PlayerData.hasWand1)
            UnlockWeapon(WeaponType.WandLevel1);

        if (PlayerData.hasWand2)
            UnlockWeapon(WeaponType.WandLevel2);

        if (PlayerData.hasWand3)
            UnlockWeapon(WeaponType.WandLevel3);

        SwitchWeapon(PlayerData.currentWeapon);

        level1Energy = PlayerData.level1Energy;
        level2Energy = PlayerData.level2Energy;
        level3Energy = PlayerData.level3Energy;

        level1ReserveEnergy = PlayerData.level1Reserve;
        level2ReserveEnergy = PlayerData.level2Reserve;
        level3ReserveEnergy = PlayerData.level3Reserve;

        OnInventoryChanged?.Invoke();
    }

    public void SaveCheckpointInventory()
    {
        checkpointSnapshot = new InventorySnapshot();

        checkpointSnapshot.level1Energy = level1Energy;
        checkpointSnapshot.level2Energy = level2Energy;
        checkpointSnapshot.level3Energy = level3Energy;

        checkpointSnapshot.level1Reserve = level1ReserveEnergy;
        checkpointSnapshot.level2Reserve = level2ReserveEnergy;
        checkpointSnapshot.level3Reserve = level3ReserveEnergy;

        checkpointSnapshot.bandages = bandageCount;
        checkpointSnapshot.elixirs = elixirCount;
        checkpointSnapshot.buffs = buffCount;

        Debug.Log("Checkpoint Inventory Saved");
    }

    public void RestoreCheckpointInventory()
    {
        if (checkpointSnapshot == null)
            return;

        level1Energy = checkpointSnapshot.level1Energy;
        level2Energy = checkpointSnapshot.level2Energy;
        level3Energy = checkpointSnapshot.level3Energy;

        level1ReserveEnergy = checkpointSnapshot.level1Reserve;
        level2ReserveEnergy = checkpointSnapshot.level2Reserve;
        level3ReserveEnergy = checkpointSnapshot.level3Reserve;

        bandageCount = checkpointSnapshot.bandages;
        elixirCount = checkpointSnapshot.elixirs;
        buffCount = checkpointSnapshot.buffs;

        OnInventoryChanged?.Invoke();

        WeaponController weapon = GetComponent<WeaponController>();

        if (weapon != null)
        {
            weapon.RefreshWeapon();
        }

        Debug.Log("Checkpoint Inventory Restored");
    }
}
