using TMPro;
using UnityEngine;

public class UpgradeStationDesc : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI upgradeName, price, upgradeDesc;

    private float startingPrice = 1000f;

    private bool selected = false;

    public void SetSelected(bool selected)
    {
        this.selected = selected;
    }

    public void SetInfo(string name, Weapon weapon)
    {
        upgradeName.text = name;
        price.text = (startingPrice + startingPrice * weapon.upgradeModuleHash[name]) + "";

        if(name.Equals("DMG"))
        {
            upgradeDesc.text = 
                weapon.damage + " >>> " + (weapon.damage + weapon.damage * weapon.upgradeModuleHash["DMG"]);
        } else if(name.Equals("MAG"))
        {
            upgradeDesc.text =
                weapon.magazineSize+ " >>> " + (weapon.magazineSize + weapon.magazineSize * weapon.upgradeModuleHash["MAG"]);
        } else if(name.Equals("ROF"))
        {
            upgradeDesc.text =
                weapon.fireRate + " >>> " + (weapon.fireRate + weapon.fireRate * weapon.upgradeModuleHash["ROF"]);
        } else
        {
            upgradeDesc.text =
                weapon.reloadTime + " >>> " + (weapon.reloadTime + weapon.reloadTime * weapon.upgradeModuleHash["REL"]);
        }
    }
}
