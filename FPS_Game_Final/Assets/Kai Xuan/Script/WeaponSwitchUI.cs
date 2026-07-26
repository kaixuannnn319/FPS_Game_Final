using UnityEngine;

public class WeaponSwitchUI : MonoBehaviour
{
    [SerializeField] private WeaponController weaponController;
    [SerializeField] private WeaponSwitchUIController uiController;

    private bool firstWeaponShown = false;

    private void Start()
    {
        if (weaponController == null)
        {
            Debug.LogError("WeaponController not assigned.");
            return;
        }

        weaponController.OnWeaponChanged.AddListener(UpdateWeaponUI);
    }

    private void UpdateWeaponUI(WeaponType weapon)
    {
        switch (weapon)
        {
            case WeaponType.None:
                uiController.HideWeaponUI();
                firstWeaponShown = false;
                break;

            case WeaponType.Knife:
                ShowWeapon(0);
                break;

            case WeaponType.WandLevel1:
                ShowWeapon(1);
                break;

            case WeaponType.WandLevel2:
                ShowWeapon(2);
                break;

            case WeaponType.WandLevel3:
                ShowWeapon(3);
                break;
        }
    }

    private void ShowWeapon(int index)
    {
        uiController.ShowWeaponUI();

        if (!firstWeaponShown)
        {
            uiController.SetWeaponImmediately(index);
            firstWeaponShown = true;
        }
        else
        {
            uiController.PlayWeaponSwitch(index);
        }
    }

    private void OnDestroy()
    {
        if (weaponController != null)
            weaponController.OnWeaponChanged.RemoveListener(UpdateWeaponUI);
    }
}