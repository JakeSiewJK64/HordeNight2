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

    public void SetInfo(Weapon weapon)
    {
        weaponName.text = weapon.name;
        weaponHolding.text = weapon.weaponHolding.ToString();
        weaponType.text = weapon.weaponType.ToString();
        damage.text = weapon.damage.ToString();
        reloadSpeed.text = weapon.reloadTime.ToString();
        rateOfFire.text = weapon.fireRate.ToString();
        headshotDamage.text = (weapon.damage * 2) + "";
    }
}
