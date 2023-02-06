using System.Collections.Generic;

public class Inventory
{
    Dictionary<string, Item> inventory { get; set; }

    public Inventory(Dictionary<string, Item> inventory)
    {
        this.inventory = inventory;
    }

    public List<Item> GetAllWeaponsFromInventory()
    {
        List<Item> weaponList = new List<Item> { inventory["Secondary"] };

        if (inventory.ContainsKey("Primary"))
        {
            weaponList.Add(inventory["Primary"]);
        }
        return weaponList;
    }

    public Item GetPrimaryWeapon()
    {
        try
        {
            return inventory["Primary"];
        }
        catch { return null; }
    }

    public Item GetSecondaryWeapon()
    {
        try
        {
            return inventory["Secondary"];
        }
        catch { return null; }
    }

    public void SetPrimaryWeapon(Weapon item)
    {
        if (inventory.ContainsKey("Primary"))
        {
            inventory.Remove("Primary");
        }
        inventory.Add("Primary", item);
    }

    public void SetSecondaryWeapon(Weapon item)
    {
        if (inventory.ContainsKey("Secondary"))
        {
            inventory.Remove("Secondary");
        }
        inventory.Add("Secondary", item);
    }
}
