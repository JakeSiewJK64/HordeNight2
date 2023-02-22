using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyStationDescription : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI weaponName,
        weaponHolding,
        weaponType,
        damage,
        reloadSpeed,
        rateOfFire,
        headshotDamage;

    [SerializeField]
    private Image weaponTypeIcon;

    private string imagePath = "Images\\";

    public void SetInfo(Weapon weapon)
    {
        weaponName.text = weapon.name;
        weaponHolding.text = weapon.weaponHolding.ToString();
        weaponType.text = weapon.weaponType.ToString();
        damage.text = "DMG\n" + weapon.damage.ToString();
        reloadSpeed.text = "REL\n" + weapon.reloadTime.ToString();
        rateOfFire.text = "ROF\n" + weapon.fireRate.ToString();
        headshotDamage.text = (weapon.damage * 2) + "";

        switch(weapon.weaponType) 
        {
            case WeaponType.Sidearm:
                weaponTypeIcon.sprite = Resources.Load<Sprite>(Path.Combine(imagePath, "handgunammo"));
                break;
            case WeaponType.Shotgun:
                weaponTypeIcon.sprite = Resources.Load<Sprite>(Path.Combine(imagePath, "shotgunshell"));
                break;
            default:
                weaponTypeIcon.sprite = Resources.Load<Sprite>(Path.Combine(imagePath, "rifleammo"));
                break;
        }
    }
}
