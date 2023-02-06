public class Weapon : Item
{
    WeaponType weaponType;
    WeaponHolding weaponHolding;
    float reserveAmmo;
    float startingAmmo;
    int damage;
    float magazineSize;
    float currentBullets;
    float fireRate;
    float reloadTime;
    string shootingSoundPath;
    string reloadingSoundPath;
    string weaponIconPath;
    string weaponPrefabPath;

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
    }

    public void ResetReserveAmmo()
    {
        reserveAmmo = startingAmmo;
    }

    public int GetMagazineSize()
    {
        return (int)magazineSize;
    }

    public string GetWeaponIconPath()
    {
        return weaponIconPath;
    }

    public void Reload()
    {
        if (reserveAmmo > 0)
        {
            if (reserveAmmo > GetMagazineSize())
            {
                if (currentBullets > 0)
                {
                    reserveAmmo -= GetMagazineSize() - currentBullets;
                    currentBullets = GetMagazineSize();
                    return;
                }
                currentBullets = GetMagazineSize();
                reserveAmmo -= GetMagazineSize();
            }
            else
            {
                currentBullets = reserveAmmo;
                reserveAmmo -= currentBullets;
            }
        }
    }
}
