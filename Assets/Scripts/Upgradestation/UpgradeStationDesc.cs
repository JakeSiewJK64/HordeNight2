using TMPro;
using UnityEngine;

public class UpgradeStationDesc : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI upgradeName, priceTM, upgradeDesc, level;

    private float startingPrice = 1000f;

    private bool selected = false;

    private string upgradeTag;

    private float price;

    public void SetSelected(bool selected)
    {
        this.selected = selected;
    }

    public string GetTag()
    {
        return upgradeTag;
    }

    public float GetPrice()
    {
        return price;
    }

    public void SetInfo(string name, Weapon weapon)
    {
        upgradeTag = name;
        upgradeName.text = name;
        price = startingPrice + startingPrice * weapon.upgradeModuleHash[name];
        priceTM.text = price + " PTS";

        if(name.Equals("DMG"))
        {
            level.text = "" + weapon.upgradeModuleHash["DMG"];
            upgradeDesc.text = 
                weapon.damage + " >>> " + (weapon.damage + weapon.damage * (weapon.upgradeModuleHash["DMG"] * .25f));
        } else if(name.Equals("MAG"))
        {
            level.text = "" + weapon.upgradeModuleHash["MAG"];
            upgradeDesc.text =
                weapon.magazineSize+ " >>> " + (weapon.magazineSize + weapon.magazineSize * (weapon.upgradeModuleHash["MAG"] * .25f));
        } else if(name.Equals("ROF"))
        {
            level.text = "" + weapon.upgradeModuleHash["ROF"];
            upgradeDesc.text =
                weapon.fireRate + " >>> " + (weapon.fireRate + weapon.fireRate * (weapon.upgradeModuleHash["ROF"] * .25f) + "s");
        } else
        {
            level.text = "" + weapon.upgradeModuleHash["REL"];
            upgradeDesc.text =
                weapon.reloadTime + " >>> " + (weapon.reloadTime - weapon.reloadTime * (weapon.upgradeModuleHash["REL"] * .25f) + "s");
        }
    }
}
