using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;

public class PlayerInventoryScript : MonoBehaviour
{
    private Inventory inventory;
    private Weapon currentWeapon;

    private string imagePath = "Images\\Weapons\\";

    [SerializeField]
    private Image[] weaponHotbar;

    private List<Weapon> playerWeapons;

    private void Awake()
    {
        InitializeInventory();
        currentWeapon = (Weapon)inventory.GetSecondaryWeapon();
        UpdateWeapons();
    }

    public void SwapWeapon(int index)
    {
        currentWeapon = playerWeapons[index];
        GetComponent<BulletSpawnScript>().ChangeWeapon(currentWeapon);
    }

    public List<Weapon> GetAllWeapons()
    {
        return playerWeapons;
    }

    public void UpdateWeapons()
    {
        playerWeapons = new List<Weapon>
        {
            (Weapon)inventory.GetPrimaryWeapon(),
            (Weapon)inventory.GetSecondaryWeapon(),
            (Weapon)inventory.GetThirdWeapon(),
        };

        for (int i = 0; i < playerWeapons.Count; i++)
        {
            if (playerWeapons[i] != null)
            {
                playerWeapons[i].currentBullets = playerWeapons[i].magazineSize;
                playerWeapons[i].reserveAmmo = playerWeapons[i].startingAmmo;
                weaponHotbar[i].GetComponent<Image>().gameObject.SetActive(true);
                weaponHotbar[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(Path.Combine(imagePath, playerWeapons[i].GetWeaponIconPath()));
            } else
            {
                weaponHotbar[i].GetComponent<Image>().gameObject.SetActive(false);
            }
        }

        switch (currentWeapon.weaponHolding)
        {
            case WeaponHolding.PRIMARY:
                SwapWeapon(0);
                break;
            case WeaponHolding.SECONDARY:
                SwapWeapon(1);
                break;
            default:
                SwapWeapon(2);
                break;
        }
    }

    private void InitializeInventory()
    {
        inventory = new Inventory(
           new Dictionary<string, Item> {                
               {
                   "Secondary",
                   new Weapon("glock 18", "description", ItemType.Weapon, WeaponType.Sidearm, WeaponHolding.SECONDARY,
                   reserveAmmo: 40,
                   startingAmmo: 40,
                   damage: 70,
                   magazineSize: 8,
                   currentBullets: 8,
                   fireRate: .2f,
                   reloadTime: 2f,
                   shootingSoundPath: "handgun_shoot",
                   reloadingSoundPath: "handgun_reload",
                   weaponIconPath: "glock",
                   weaponPrefabPath: "glock18",
                   price: 1000)
               },
           }
        );
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public Inventory GetPlayerInventory()
    {
        return inventory;
    }
}
