using UnityEngine;
public enum WeaponLevel
{
    None,
    Knife,
    Level1Weapon,
    Level2Weapon,
    Level3Weapon
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
    [SerializeField] private bool hasLevel1Weapon = false;
    [SerializeField] private bool hasLevel2Weapon = false;
    [SerializeField] private bool hasLevel3Weapon = false;

    [SerializeField] private WeaponLevel currentWeapon = WeaponLevel.None;

    [Header("Quest Items")]
    [SerializeField] private int keyCount = 0;
    [SerializeField] private int relicCount = 0;
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

    public WeaponLevel GetCurrentWeapon()
    {
        return currentWeapon;
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
        return hasLevel1Weapon;
    }

    public bool HasLevel2Weapon()
    {
        return hasLevel2Weapon;
    }

    public bool HasLevel3Weapon()
    {
        return hasLevel3Weapon;
    }

    public int GetMaxBandage()
    {
        return maxBandage;
    }

    public int GetMaxElixir()
    {
        return maxElixir;
    }
    public int GetKeyCount()
    {
        return keyCount;
    }

    public int GetRelicCount()
    {
        return relicCount;
    }

    public int GetMaxBuff()
    {
        return maxBuff;
    }
    public void UnlockWeapon(WeaponLevel weapon)
    {
        switch (weapon)
        {
            case WeaponLevel.Knife:
                hasKnife = true;
                break;

            case WeaponLevel.Level1Weapon:
                hasLevel1Weapon = true;
                break;

            case WeaponLevel.Level2Weapon:
                hasLevel2Weapon = true;
                break;

            case WeaponLevel.Level3Weapon:
                hasLevel3Weapon = true;
                break;
        }

        Debug.Log("Unlocked : " + weapon);
    }



    public bool SwitchWeapon(WeaponLevel weapon)
    {
        switch (weapon)
        {
            case WeaponLevel.Knife:
                if (!hasKnife) return false;
                break;

            case WeaponLevel.Level1Weapon:
                if (!hasLevel1Weapon) return false;
                break;

            case WeaponLevel.Level2Weapon:
                if (!hasLevel2Weapon) return false;
                break;

            case WeaponLevel.Level3Weapon:
                if (!hasLevel3Weapon) return false;
                break;
        }

        currentWeapon = weapon;
        Debug.Log("Current Weapon : " + currentWeapon);
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

    public void AddKey()
    {
        keyCount++;
    }

    public void AddRelic()
    {
        relicCount++;
    }

}
