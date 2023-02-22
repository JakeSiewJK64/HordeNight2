using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyStationViewholder : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI weaponName, damage, rateOfFire, magazineSize, price, reloadSpeed;

    [SerializeField]
    private Image weaponProfile;

    private Weapon selectedWeapon;

    public void SetInfo(ViewholderItems item)
    {
        weaponName.text = item.name;
        damage.text = "DMG\n" + item.damage;
        rateOfFire.text = "ROF\n" + item.rateOfFire + "s";
        magazineSize.text = "MAG\n" + item.magazineSize;
        reloadSpeed.text = "REL\n" + item.reloadSpeed + "s";
        price.text = item.points + " pts";
        weaponProfile.sprite = item.weaponProfile;
        selectedWeapon = item.weapon;
    }

    public void OnBuyButtonPressed()
    {
        GetComponentInParent<BuystationScript>().UpdateDescription(selectedWeapon);
    }
}
