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
    // Index convention used throughout this script for the three wand levels: 0 = Level1, 1 = Level2, 2 = Level3.

    [Header("Energy Pickup Amount")]
    [SerializeField] private float[] pickupAmount = { 10f, 5f, 2f };

    [Header("Magic Energy")]
    [SerializeField] private float[] energy = { 100f, 100f, 100f };
    [SerializeField] private float[] reserveEnergy = { 0f, 0f, 0f };
    [SerializeField] private float[] maxReserveEnergy = { 200f, 400f, 360f };

    [Header("Healing Items")]
    [SerializeField] private int bandageCount = 0;
    [SerializeField] private int elixirCount = 0;
    [SerializeField] private int maxBandage = 3;
    [SerializeField] private int maxElixir = 1;

    [Header("Weapon Unlock")]
    [SerializeField] private bool hasKnife = false;
    [SerializeField] private bool[] hasWandLevel = { false, false, false };
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

    private void Start()
    {
        LoadInventory();
    }

    // ---- Weapon level index helper ----
    // Maps WandLevel1/2/3 to 0/1/2. Returns -1 for anything else (None, Knife).
    private int LevelIndex(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.WandLevel1: return 0;
            case WeaponType.WandLevel2: return 1;
            case WeaponType.WandLevel3: return 2;
            default: return -1;
        }
    }

    // ---- Getters ----
    public int GetBuffCount() => buffCount;
    public WeaponType GetCurrentWeaponType() => currentWeaponType;

    public float GetLevel1Energy() => energy[0];
    public float GetLevel2Energy() => energy[1];
    public float GetLevel3Energy() => energy[2];

    public float GetLevel1ReserveEnergy() => reserveEnergy[0];
    public float GetLevel2ReserveEnergy() => reserveEnergy[1];
    public float GetLevel3ReserveEnergy() => reserveEnergy[2];

    public int GetBandageCount() => bandageCount;
    public int GetElixirCount() => elixirCount;

    public bool HasKnife() => hasKnife;
    public bool HasLevel1Weapon() => hasWandLevel[0];
    public bool HasLevel2Weapon() => hasWandLevel[1];
    public bool HasLevel3Weapon() => hasWandLevel[2];

    public int GetMaxBandage() => maxBandage;
    public int GetMaxElixir() => maxElixir;
    public int GetRelicCount() => sealCount;
    public int GetMaxBuff() => maxBuff;

    // ---- Weapons ----
    public void UnlockWeapon(WeaponType weapon)
    {
        if (weapon == WeaponType.Knife)
        {
            hasKnife = true;
        }
        else
        {
            int i = LevelIndex(weapon);
            if (i >= 0) hasWandLevel[i] = true;
        }

        OnInventoryChanged?.Invoke();
        SaveInventory();

        Debug.Log("Unlocked : " + weapon);
    }

    public bool SwitchWeapon(WeaponType weapon)
    {
        bool unlocked = weapon switch
        {
            WeaponType.Knife => hasKnife,
            WeaponType.WandLevel1 => hasWandLevel[0],
            WeaponType.WandLevel2 => hasWandLevel[1],
            WeaponType.WandLevel3 => hasWandLevel[2],
            _ => true // WeaponType.None always allowed
        };

        if (!unlocked) return false;

        currentWeaponType = weapon;
        Debug.Log("Current Weapon : " + currentWeaponType);
        return true;
    }

    // ---- Reserve energy pickups ----
    public bool AddLevel1ReserveEnergy() => AddReserveEnergy(0);
    public bool AddLevel2ReserveEnergy() => AddReserveEnergy(1);
    public bool AddLevel3ReserveEnergy() => AddReserveEnergy(2);

    private bool AddReserveEnergy(int i)
    {
        if (reserveEnergy[i] >= maxReserveEnergy[i]) return false;

        reserveEnergy[i] = Mathf.Min(reserveEnergy[i] + pickupAmount[i], maxReserveEnergy[i]);
        OnInventoryChanged?.Invoke();
        return true;
    }

    // ---- Healing items ----
    public bool AddBandage()
    {
        if (bandageCount >= maxBandage) return false;
        bandageCount++;
        OnInventoryChanged?.Invoke();
        return true;
    }

    public bool UseBandage()
    {
        if (bandageCount <= 0) return false;
        bandageCount--;
        OnInventoryChanged?.Invoke();
        Debug.Log("Bandage Left : " + bandageCount);
        return true;
    }

    public bool AddElixir()
    {
        if (elixirCount >= maxElixir) return false;
        elixirCount++;
        OnInventoryChanged?.Invoke();
        return true;
    }

    public bool UseElixir()
    {
        if (elixirCount <= 0) return false;
        elixirCount--;
        OnInventoryChanged?.Invoke();
        Debug.Log("Elixir Left : " + elixirCount);
        return true;
    }

    public bool AddBuff()
    {
        if (buffCount >= maxBuff) return false;
        buffCount++;
        OnInventoryChanged?.Invoke();
        return true;
    }

    public bool UseBuff()
    {
        if (buffCount <= 0) return false;
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

    // ---- Quest keys ----
    public void CollectKey(string keyID)
    {
        if (collectedKeys.Contains(keyID)) return;
        collectedKeys.Add(keyID);

        switch (keyID)
        {
            case "Key1": hasKey1 = true; break;
            case "Key2": hasKey2 = true; break;
            case "Key3": hasKey3 = true; break;
        }

        Debug.Log("Collected Key : " + keyID);
        SaveInventory();
    }

    public bool HasKey(string keyID) => collectedKeys.Contains(keyID);

    // ---- Weapon energy usage / reload ----
    public bool UseEnergy(WeaponType weaponType, float amount)
    {
        int i = LevelIndex(weaponType);
        if (i < 0 || energy[i] < amount) return false;

        energy[i] -= amount;
        OnInventoryChanged?.Invoke();
        return true;
    }

    public bool ReloadEnergy(WeaponType weaponType)
    {
        int i = LevelIndex(weaponType);
        if (i < 0 || reserveEnergy[i] <= 0) return false;

        float need = 100f - energy[i];
        float give = Mathf.Min(need, reserveEnergy[i]);

        energy[i] += give;
        reserveEnergy[i] -= give;

        OnInventoryChanged?.Invoke();
        Debug.Log($"Level{i + 1} Reload : {energy[i]}/100 | Reserve : {reserveEnergy[i]}");
        return true;
    }

    public float TakeReserveEnergy(WeaponType weaponType, float amount)
    {
        int i = LevelIndex(weaponType);
        if (i < 0) return 0f;

        float give = Mathf.Min(amount, reserveEnergy[i]);
        reserveEnergy[i] -= give;
        OnInventoryChanged?.Invoke();
        return give;
    }

    // ---- Save / Load ----
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
        if (!PlayerData.hasSave) return;

        if (PlayerData.hasKnife) UnlockWeapon(WeaponType.Knife);
        if (PlayerData.hasWand1) UnlockWeapon(WeaponType.WandLevel1);
        if (PlayerData.hasWand2) UnlockWeapon(WeaponType.WandLevel2);
        if (PlayerData.hasWand3) UnlockWeapon(WeaponType.WandLevel3);

        SwitchWeapon(PlayerData.currentWeapon);

        energy[0] = PlayerData.level1Energy;
        energy[1] = PlayerData.level2Energy;
        energy[2] = PlayerData.level3Energy;

        reserveEnergy[0] = PlayerData.level1Reserve;
        reserveEnergy[1] = PlayerData.level2Reserve;
        reserveEnergy[2] = PlayerData.level3Reserve;

        OnInventoryChanged?.Invoke();
    }

    public void SaveCheckpointInventory()
    {
        checkpointSnapshot = new InventorySnapshot
        {
            level1Energy = energy[0],
            level2Energy = energy[1],
            level3Energy = energy[2],

            level1Reserve = reserveEnergy[0],
            level2Reserve = reserveEnergy[1],
            level3Reserve = reserveEnergy[2],

            bandages = bandageCount,
            elixirs = elixirCount,
            buffs = buffCount
        };

        Debug.Log("Checkpoint Inventory Saved");
    }

    public void RestoreCheckpointInventory()
    {
        if (checkpointSnapshot == null) return;

        energy[0] = checkpointSnapshot.level1Energy;
        energy[1] = checkpointSnapshot.level2Energy;
        energy[2] = checkpointSnapshot.level3Energy;

        reserveEnergy[0] = checkpointSnapshot.level1Reserve;
        reserveEnergy[1] = checkpointSnapshot.level2Reserve;
        reserveEnergy[2] = checkpointSnapshot.level3Reserve;

        bandageCount = checkpointSnapshot.bandages;
        elixirCount = checkpointSnapshot.elixirs;
        buffCount = checkpointSnapshot.buffs;

        OnInventoryChanged?.Invoke();

        WeaponController weapon = GetComponent<WeaponController>();
        if (weapon != null)
            weapon.RefreshWeapon();

        Debug.Log("Checkpoint Inventory Restored");
    }
}