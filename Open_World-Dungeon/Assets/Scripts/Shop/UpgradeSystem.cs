using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeSystem : MonoBehaviour
{
    //GunButton
    public BasicGunBehaviour Gun1, Gun2;
    public Button Gun1Button, Gun2Button;

    //Current Stats Screen
    public TextMeshProUGUI DamageT, FireRateT, MaxAmmoT, ReloadTimeT, RangeT;

    //Upgrade Buttons
    public Button DamageU, FireRateU, MaxAmmoU, ReloadTimeU, RangeU;

    public int DamagePerUpgrade, MaxAmmoPerUpgrade, RangePerUpgrade;
    public float FireRatePerUpgrade, ReloadTimePerUpgrade;

    BasicGunBehaviour currentGun;

    public Inventory inv;

    private void Start()
    {
        if (Gun1) Gun1Button.onClick.AddListener(delegate { SetStatsDisplay(Gun1); });
        if (Gun2) Gun2Button.onClick.AddListener(delegate { SetStatsDisplay(Gun2); });

        DamageU.onClick.AddListener(UpgradeDamage);
        FireRateU.onClick.AddListener(UpgradeFireRate);
        MaxAmmoU.onClick.AddListener(UpgradeMaxAmmo);
        ReloadTimeU.onClick.AddListener(UpgradeReloadTime);
        RangeU.onClick.AddListener(UpgradeRange);

    }

    void SetStatsDisplay(BasicGunBehaviour gun)
    {
        DamageT.text = gun.Damage.ToString();
        FireRateT.text = gun.FireRate.ToString();
        MaxAmmoT.text = gun.MaxAmmo.ToString();
        ReloadTimeT.text = gun.ReloadTime.ToString();
        RangeT.text = gun.Range.ToString();
        currentGun = gun;
    }

    void UpgradeDamage()
    {
        if (currentGun.DamageUpgrade.Count <= 0) Debug.Log("No more upgrades");
        else if (CompareInventory(currentGun.DamageUpgrade[0]))
        {
            currentGun.Damage += ((int)currentGun.DamageUpgrade[0].Amount);
            inv.RemoveAmount(currentGun.DamageUpgrade[0]);
            currentGun.DamageUpgrade.RemoveAt(0);
            SetStatsDisplay(currentGun);
        }
    }
    void UpgradeFireRate()
    {
        if (CompareInventory(currentGun.FireRateUpgrade[0]))
        {
            currentGun.FireRate -= currentGun.FireRateUpgrade[0].Amount;
            currentGun.FireRateUpgrade.RemoveAt(0);
            SetStatsDisplay(currentGun);
        }
    }
    void UpgradeMaxAmmo()
    {
        if (CompareInventory(currentGun.MaxAmmoUpgrade[0]))
        {
            currentGun.MaxAmmo += (int)currentGun.MaxAmmoUpgrade[0].Amount;
            currentGun.MaxAmmoUpgrade.RemoveAt(0);
            SetStatsDisplay(currentGun);
        }
    }
    void UpgradeReloadTime()
    {
        if (CompareInventory(currentGun.ReloadTimeUpgrade[0]))
        {
            currentGun.ReloadTime -= currentGun.ReloadTimeUpgrade[0].Amount;
            currentGun.ReloadTimeUpgrade.RemoveAt(0);
            SetStatsDisplay(currentGun);
        }
    }
    void UpgradeRange()
    {
        if (CompareInventory(currentGun.RangeUpgrade[0]))
        {
            currentGun.Range += (int)currentGun.RangeUpgrade[0].Amount;
            currentGun.RangeUpgrade.RemoveAt(0);
            SetStatsDisplay(currentGun);
        }
    }

    bool CompareInventory(UpgradeCost cost)
    {
        foreach (var item in cost.cost)
        {
            if (inv.items.ContainsKey(item.loot))
            {
                if (inv.items[item.loot].GetComponent<InventoryItem>().Quantitiy >= item.amount) { }
                else
                {
                    Debug.Log("Not enough of the item in inventory");
                    return false;

                }
            }
            else
            {
                Debug.Log("Item Not in inventory");
                return false;
            }
        }
        return true;
    }
}
