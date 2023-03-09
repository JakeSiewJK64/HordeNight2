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

    private int weaponIndex = 0;

    private void Awake()
    {
        InitializeInventory();
        currentWeapon = (Weapon)inventory.GetSecondaryWeapon();
        UpdateWeapons();
    }

    public int GetWeaponIndex()
    {
        return weaponIndex;
    }

    public bool ContainsWeapon(string weaponName)
    {
        foreach(var weapon in playerWeapons) 
        {
            if(weapon != null && weapon.name == weaponName) return true;
        }
        return false;
    }

    public void SwapWeapon(int index)
    {
        weaponIndex = index;
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
                weaponHotbar[i].GetComponent<Image>().gameObject.SetActive(true);
                weaponHotbar[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(Path.Combine(imagePath, playerWeapons[i].GetWeaponIconPath()));
            } else
            {
                weaponHotbar[i].GetComponent<Image>().gameObject.SetActive(false);
            }
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
