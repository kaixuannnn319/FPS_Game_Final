using UnityEngine;
using System.Collections.Generic;
public enum WeaponType
{
    None,
    Knife,
    WandLevel1,
    WandLevel2,
    WandLevel3
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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

        return true;
    }
    public bool AddLevel2ReserveEnergy()
    {
        if (level2ReserveEnergy >= maxLevel2ReserveEnergy)
            return false;

        level2ReserveEnergy += level2PickupAmount;

        level2ReserveEnergy = Mathf.Min(level2ReserveEnergy, maxLevel2ReserveEnergy);

        return true;
    }

    public bool AddLevel3ReserveEnergy()
    {
        if (level3ReserveEnergy >= maxLevel3ReserveEnergy)
            return false;

        level3ReserveEnergy += level3PickupAmount;

        level3ReserveEnergy = Mathf.Min(level3ReserveEnergy, maxLevel3ReserveEnergy);

        return true;
    }

    public bool AddBandage()
    {
        if (bandageCount >= maxBandage)
            return false;

        bandageCount++;
        return true;
    }

    public bool UseBandage()
    {
        if (bandageCount <= 0)
            return false;

        bandageCount--;
        return true;
    }

    public bool AddElixir()
    {
        if (elixirCount >= maxElixir)
            return false;

        elixirCount++;
        return true;
    }

    public bool UseElixir()
    {
        if (elixirCount <= 0)
            return false;

        elixirCount--;
        return true;
    }
    public bool AddBuff()
    {
        if (buffCount >= maxBuff)
            return false;

        buffCount++;
        return true;
    }

    public bool UseBuff()
    {
        if (buffCount <= 0)
            return false;

        buffCount--;
        return true;
    }

    public void AddRelic()
    {
        sealCount++;
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
    }

    public bool HasKey(string keyID)
    {
        return collectedKeys.Contains(keyID);
    }


}
