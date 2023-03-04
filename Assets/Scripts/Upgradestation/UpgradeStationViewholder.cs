using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeStationViewholder : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI weaponName, damage, rateOfFire, magazineSize, reloadSpeed;

    [SerializeField]
    private Image weaponProfile;

    private Weapon selectedWeapon;

    private string id;

    private bool selected;

    public void SetSelected(bool selected)
    {
        this.selected = selected;
        if (selected)
        {
            OnBuyButtonPressed();
        }
    }

    public void SetInfo(ViewholderItems item)
    {
        weaponName.text = item.name;
        damage.text = "DMG\n" + item.damage;
        rateOfFire.text = "ROF\n" + item.rateOfFire + "s";
        magazineSize.text = "MAG\n" + item.magazineSize;
        reloadSpeed.text = "REL\n" + item.reloadSpeed + "s";
        weaponProfile.sprite = item.weaponProfile;
        selectedWeapon = item.weapon;
        id = item.name + item.points;
    }

    public string GetId()
    {
        return id;
    }

    public void OnBuyButtonPressed()
    {
        GetComponentInParent<UpgradeStationScript>().UpdateDescription(selectedWeapon);
    }
}
