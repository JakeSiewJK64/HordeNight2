using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;

public class PlayerInventoryScript : MonoBehaviour
{
    private Inventory inventory;
    private Weapon currentWeapon;

    private string imagePath = "Raw\\Img\\";

    [SerializeField]
    private Image primaryWeaponHotBar;

    [SerializeField]
    private Image secondaryWeaponHotBar;

    private void Awake()
    {
        InitializeInventory();
        currentWeapon = (Weapon)inventory.GetSecondaryWeapon();
        UpdateWeaponHotbarSprites();
    }

    public void UpdateWeaponHotbarSprites()
    {
        if (inventory.GetPrimaryWeapon() != null)
        {
            primaryWeaponHotBar.GetComponent<Image>().sprite = Resources.Load<Sprite>(Path.Combine(imagePath, ((Weapon)inventory.GetPrimaryWeapon()).GetWeaponIconPath()));
        }
        else if (inventory.GetSecondaryWeapon() != null)
        {
            secondaryWeaponHotBar.GetComponent<Image>().sprite = Resources.Load<Sprite>(Path.Combine(imagePath, ((Weapon)inventory.GetSecondaryWeapon()).GetWeaponIconPath()));
        }
    }

    private void InitializeInventory()
    {
        inventory = new Inventory(
           new Dictionary<string, Item> {
                {
                   "Secondary",
                   new Weapon("glock 18", "description", ItemType.Weapon, WeaponType.Sidearm, WeaponHolding.SECONDARY, 40, startingAmmo: 40, damage: 70, 8, 8, .2f, 2f, "glock", "glock_reload", "glock", "glock18", 1000)
               }
           }
        );
    }

    private void CheckInput()
    {
    }

    private void Update()
    {
        CheckInput();
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