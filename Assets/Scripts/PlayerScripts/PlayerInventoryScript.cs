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
        playerWeapons = new List<Weapon>
        {
            (Weapon)inventory.GetPrimaryWeapon(),
            (Weapon)inventory.GetSecondaryWeapon(),
            (Weapon)inventory.GetThirdWeapon(),
        };
        UpdateWeaponHotbarSprites();
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

    public void UpdateWeaponHotbarSprites()
    {
        for(int i = 0; i < playerWeapons.Count; i++)
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
