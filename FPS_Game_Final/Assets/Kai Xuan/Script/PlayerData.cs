using System.Collections.Generic;

public static class PlayerData
{
    public static bool hasSave = false;

    // Weapons
    public static bool hasKnife;
    public static bool hasWand1;
    public static bool hasWand2;
    public static bool hasWand3;

    public static WeaponType currentWeapon;

    // Energy
    public static float level1Energy;
    public static float level2Energy;
    public static float level3Energy;

    public static float level1Reserve;
    public static float level2Reserve;
    public static float level3Reserve;

    public static bool hasKey1;
    public static bool hasKey2;
    public static bool hasKey3;

    public static int relicCount;

    // Later:
    public static List<string> unlockedClues;
}