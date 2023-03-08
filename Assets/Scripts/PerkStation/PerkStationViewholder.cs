using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerkStationViewholder : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI upgradeNameTM, upgradeChangeTM, levelTM, priceTM;

    [SerializeField]
    private Image upgradeProfile;

    private string baseURL = "Images\\";

    private KeyValuePair<string, int> upgrade;

    private float startingPrice = 1000;

    public KeyValuePair<string, int> GetUpgrade()
    {
        return upgrade;
    }

    public float GetPrice()
    {
        return startingPrice;
    }

    public void SetInfo(KeyValuePair <string, int> perkUpgrade, Player player)
    {
        upgrade = perkUpgrade;
        startingPrice += startingPrice * player.upgradeModule[upgrade.Key];

        if (perkUpgrade.Key == "MaximumHealth")
        {
            SetViewholder(
                perkUpgrade.Key,
                player.GetMaxHealth() + " >>> " + (player.GetMaxHealth() + player.GetMaxHealth() * ((player.upgradeModule[perkUpgrade.Key] + 1) * .05f)),
                Resources.Load<Sprite>(Path.Combine(baseURL, "health_icon")), player.upgradeModule[perkUpgrade.Key].ToString());
        }
        else if (perkUpgrade.Key == "MovementSpeed")
        {
            SetViewholder(
                perkUpgrade.Key,
                player.GetSpeed() + " >>> " + (player.GetSpeed() + player.GetSpeed() * ((player.upgradeModule[perkUpgrade.Key] + 1) * .1f)),
                Resources.Load<Sprite>(Path.Combine(baseURL, "run")), player.upgradeModule[perkUpgrade.Key].ToString());
        }
        else if (perkUpgrade.Key == "HealthRegenSpeed")
        {
            SetViewholder(
                perkUpgrade.Key,
                player.GetHealthRegenSpeed() + " >>> " + (player.upgradeModule[perkUpgrade.Key] + 1) * .25f,
                Resources.Load<Sprite>(Path.Combine(baseURL, "health_icon")), player.upgradeModule[perkUpgrade.Key].ToString());
        } else if (perkUpgrade.Key == "HealthRegenDelay")
        {
            SetViewholder(
                perkUpgrade.Key,
                player.GetHealthRegenDelay() + " >>> " + (player.GetHealthRegenDelay() + player.GetHealthRegenDelay() * ((player.upgradeModule[perkUpgrade.Key] + 1) * .15f)),
                Resources.Load<Sprite>(Path.Combine(baseURL, "health_icon")), player.upgradeModule[perkUpgrade.Key].ToString());
        }
    }

    private void SetViewholder(string key, string val, Sprite sprite, string level) 
    {
        upgradeNameTM.text = key;
        upgradeChangeTM.text = val;
        upgradeProfile.sprite = sprite;
        levelTM.text = "Level " + level;
        priceTM.text = startingPrice + " PTS";
    }

    public void SetSelected(bool select)
    {
        if (select)
        {
            OnBuyButtonPressed();
        }
    }

    public void OnBuyButtonPressed()
    {
    }
}
