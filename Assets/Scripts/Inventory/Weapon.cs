using System;
using System.Collections.Generic;

public class Weapon : Item
{
    public Dictionary<string, int> upgradeModuleHash { get; set; }
    public WeaponType weaponType { get; set; }
    public WeaponHolding weaponHolding { get; set; }
    public float reserveAmmo { get; set; }
    public float startingAmmo { get; set; }
    public int damage { get; set; }
    public float magazineSize { get; set; }
    public float currentBullets { get; set; }
    public float fireRate { get; set; }
    public float reloadTime { get; set; }
    public string shootingSoundPath { get; set; }
    public string reloadingSoundPath { get; set; }
    public string weaponIconPath { get; set; }
    public string weaponPrefabPath { get; set; }

    public Weapon(
        string name,
        string description,
        ItemType itemType,
        WeaponType weaponType,
        WeaponHolding weaponHolding,
        float reserveAmmo,
        float startingAmmo,
        int damage,
        float magazineSize,
        float currentBullets,
        float fireRate,
        float reloadTime,
        string shootingSoundPath,
        string reloadingSoundPath,
        string weaponIconPath,
        string weaponPrefabPath,
        float price) : base(name, description, itemType, price)
    {
        this.weaponType = weaponType;
        this.startingAmmo = startingAmmo;
        this.weaponHolding = weaponHolding;
        this.reserveAmmo = reserveAmmo;
        this.damage = damage;
        this.magazineSize = magazineSize;
        this.currentBullets = currentBullets;
        this.fireRate = fireRate;
        this.reloadTime = reloadTime;
        this.shootingSoundPath = shootingSoundPath;
        this.reloadingSoundPath = reloadingSoundPath;
        this.weaponIconPath = weaponIconPath;
        this.weaponPrefabPath = weaponPrefabPath;
        upgradeModuleHash = new Dictionary<string, int> {
            { "DMG", 0 },
            { "REL", 0 },
            { "MAG", 0 },
            { "ROF", 0 },
        };
    }

    public void LevelUpModule(string tag)
    {
        if(upgradeModuleHash.ContainsKey(tag) && upgradeModuleHash[tag] < 5)
        {
            upgradeModuleHash[tag] += 1;
        }
    }

    public void ResetReserveAmmo()
    {
        reserveAmmo = startingAmmo;
    }

    public int GetMagazineSize()
    {
        return (int)((int) magazineSize + (int) magazineSize * (upgradeModuleHash["MAG"] * .25f));
    }

    public float GetFireRate()
    {
        return fireRate - fireRate * (upgradeModuleHash["ROF"] * .03125f);
    }

    public float GetDamage()
    {
        return damage + damage * (upgradeModuleHash["DMG"] * .25f);
    }

    public float GetReloadSpeed()
    {
        return reloadTime - reloadTime * (upgradeModuleHash["REL"] * .03125f);
    }
    
    public string GetWeaponIconPath()
    {
        return weaponIconPath;
    }

    public void Reload()
    {
        if (reserveAmmo > 0)
        {
            if (reserveAmmo >= GetMagazineSize())
            {
                // non empty 
                if (currentBullets > 0)
                {
                    reserveAmmo -= GetMagazineSize() - currentBullets;
                    currentBullets = GetMagazineSize();
                    return;
                }
                // empty reload
                currentBullets = GetMagazineSize();
                reserveAmmo -= GetMagazineSize();
            }
            else
            {
                float beforeReload = currentBullets;
                float afterReload = currentBullets + reserveAmmo;
                
                if(afterReload > GetMagazineSize())
                {                    
                    afterReload = GetMagazineSize();
                }

                float difference = Math.Abs(afterReload - beforeReload);
                
                currentBullets = afterReload;
                reserveAmmo -= difference;
                return;
            }
        }
    }
}
