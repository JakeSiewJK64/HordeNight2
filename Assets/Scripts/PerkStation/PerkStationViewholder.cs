using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerkStationViewholder : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI upgradeNameTM, upgradeChangeTM;

    [SerializeField]
    private Image upgradeProfile;

    private Player player;

    private string baseURL = "Images\\";

    private bool selected = false;

    private KeyValuePair<string, int> upgrade;

    public KeyValuePair<string, int> GetUpgrade()
    {
        return upgrade;
    }

    public void SetInfo(KeyValuePair <string, int> perkUpgrade, Player player)
    {
        upgrade = perkUpgrade;

        if(player == null)
        {
            this.player= player;
        }

        if(perkUpgrade.Key == "HEALTH")
        {
            SetViewholder(
                perkUpgrade.Key, 
                player.GetHealth() + " >>> " + (player.GetHealth() + player.GetHealth() * ((player.upgradeModule[perkUpgrade.Key] + 1) * .25f)), 
                Resources.Load<Sprite>(Path.Combine(baseURL, "health_icon")));
        } else if (perkUpgrade.Key == "MOVEMENT_SPEED")
        {
            SetViewholder(
                perkUpgrade.Key, 
                player.GetSpeed() + " >>> " + (player.GetSpeed() + player.GetSpeed() * ((player.upgradeModule[perkUpgrade.Key] + 1) * .25f)),
                Resources.Load<Sprite>(Path.Combine(baseURL, "run")));
        }
    }

    private void SetViewholder(string key, string val, Sprite sprite) 
    {
        upgradeNameTM.text = key;
        upgradeChangeTM.text = val;
        upgradeProfile.sprite = sprite;
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
