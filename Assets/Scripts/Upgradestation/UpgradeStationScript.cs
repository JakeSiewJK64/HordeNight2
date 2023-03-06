using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeStationScript : MonoBehaviour
{
    [SerializeField]
    private GameObject UI, viewholder, upgradeDescViewholder;

    [SerializeField]
    private ScrollRect scrollArea, upgradeDescScrollArea;

    [SerializeField]
    private TextMeshProUGUI playerPoints;

    [SerializeField]
    private GameObject descriptionViewholder;

    private Color selectedColor = new Color(1, 0.427451f, 0.0627451f, 1);
    private Color defaultColor = Color.clear;

    private int selectedItemIndex = 0;
    private int upgradeSelectedItemIndex = 0;

    private GameObject player;
    private string imagePath = "Images\\Weapons\\";
    private List<Weapon> weapons = new List<Weapon>();

    private Weapon selectedItem = null;

    private bool selectedUpgradeItem = false;

    private void Start()
    {
        UI.SetActive(false);
        descriptionViewholder.SetActive(false);
    }

    void SetSelected(int index, ScrollRect scrollArea)
    {
        if(selectedUpgradeItem)
        {
            for (int i = 0; i < scrollArea.content.childCount; i++)
            {
                if (i == index)
                {
                    scrollArea.content.GetChild(i).GetComponent<UpgradeStationDesc>().SetSelected(true);
                    scrollArea.content.GetChild(i).GetComponent<Image>().color = selectedColor;
                }
                else
                {
                    scrollArea.content.GetChild(i).GetComponent<UpgradeStationDesc>().SetSelected(false);
                    scrollArea.content.GetChild(i).GetComponent<Image>().color = defaultColor;
                }
            }
            return;
        }

        for (int i = 0; i < scrollArea.content.childCount; i++)
        {
            if (i == index)
            {
                scrollArea.content.GetChild(i).GetComponent<UpgradeStationViewholder>().SetSelected(true);
                scrollArea.content.GetChild(i).GetComponent<Image>().color = selectedColor;
            }
            else
            {
                scrollArea.content.GetChild(i).GetComponent<UpgradeStationViewholder>().SetSelected(false);
                scrollArea.content.GetChild(i).GetComponent<Image>().color = defaultColor;
            }
        }
    }

    private void ClearUpgradeDesc()
    {
        foreach (Transform child in upgradeDescScrollArea.content)
        {
            if (child != null)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void CheckPlayerInteraction()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > 5)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            UI.SetActive(false);
            player.gameObject.GetComponent<PlayerBuyStationInteraction>().SetInteracting(false);
            player.gameObject.GetComponent<InteractScript>().SetTM("");
            descriptionViewholder.SetActive(false);
            selectedItem = null;
            selectedUpgradeItem = false;
            weapons = new List<Weapon>() { };
            player = null;

            foreach (Transform child in scrollArea.content)
            {
                if (child != null)
                {
                    Destroy(child.gameObject);
                }
            }

            ClearUpgradeDesc();
        }
        else
        {
            if (Input.GetKey(KeyCode.F))
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                UI.SetActive(true);
                ListPlayerWeapons();
                player.gameObject.GetComponent<PlayerBuyStationInteraction>().SetInteracting(true);
                playerPoints.text = player.GetComponent<PlayerPointsScript>().GetPoints() + " PTS";
                player.gameObject.GetComponent<InteractScript>().SetTM("");
            }
        }
    }

    private void Update()
    {
        if (player)
        {
            if(selectedItem != null)
            {
                CheckBuyInput();
            }

            if(selectedUpgradeItem)
            {
                ControlScrollArea(upgradeDescScrollArea, upgradeSelectedItemIndex);
            } else
            {
                ControlScrollArea(scrollArea, selectedItemIndex);
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                selectedUpgradeItem = false;
                descriptionViewholder.SetActive(false);
            }

            CheckPlayerInteraction();
        }
    }

    private void ControlScrollArea(ScrollRect scrollArea, int selectedItemIndex)
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedItemIndex = Mathf.Min(selectedItemIndex + 1, scrollArea.content.childCount - 1);
            SetSelected(selectedItemIndex, scrollArea);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedItemIndex = Mathf.Max(selectedItemIndex - 1, 0);
            SetSelected(selectedItemIndex, scrollArea);
        }

        // scroll based on index
        int index = selectedItemIndex;
        upgradeSelectedItemIndex = index;
        float normalizedPosition = (float)index / (scrollArea.content.childCount - 1);
        scrollArea.normalizedPosition = new Vector2(0, 1 - normalizedPosition);
    }

    private void InitializeViewholders()
    {
        foreach(var item in weapons)
        {
            if(item != null)
            {
                GameObject newItem = Instantiate(viewholder, scrollArea.content);
                if (newItem.TryGetComponent(out UpgradeStationViewholder viewholderItem))
                {
                    viewholderItem.SetInfo(new ViewholderItems(
                        name: item.name,
                        damage: item.damage.ToString(),
                        rateOfFire: item.fireRate.ToString(),
                        reloadSpeed: item.reloadTime.ToString(),
                        magazineSize: item.magazineSize.ToString(),
                        points: "",
                        weaponProfile: Resources.Load<Sprite>(Path.Combine(imagePath, item.weaponIconPath.ToString())),
                        weapon: item));
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            if (!player.GetComponent<PlayerBuyStationInteraction>().GetInteracting())
            {
                collision
                    .gameObject
                    .GetComponent<InteractScript>()
                    .SetTM("[ F ] to interact with upgrade station");
            }
        }
    }

    public void UpdateDescription(Weapon weapon)
    {
        ClearUpgradeDesc();
        descriptionViewholder.SetActive(true);
        foreach (var upgrade in weapon.upgradeModuleHash)
        {
            GameObject newItem = Instantiate(upgradeDescViewholder, upgradeDescScrollArea.content);
            if (newItem.TryGetComponent(out UpgradeStationDesc viewholderItem))
            {
                viewholderItem.SetInfo(upgrade.Key, weapon);
            }
        }
        selectedItem = weapon;
    }

    private void CheckBuyInput()
    {
        if 
        (
            Input.GetKeyDown(KeyCode.E) && 
            player.GetComponent<PlayerBuyStationInteraction>().GetInteracting()
        )
        {
            if (!selectedUpgradeItem)
            {
                descriptionViewholder.SetActive(true);
                selectedUpgradeItem = true;
                upgradeSelectedItemIndex = 0;
                SetSelected(upgradeSelectedItemIndex, upgradeDescScrollArea);
            }
            else
            {
                UpgradeStationDesc selectedUpgrade = upgradeDescScrollArea.content.GetChild(upgradeSelectedItemIndex).GetComponent<UpgradeStationDesc>();
                
                if(player.GetComponent<PlayerPointsScript>().GetPoints() >= selectedUpgrade.GetPrice())
                {
                    player.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sound\\upgrade"));
                    player.GetComponent<PlayerPointsScript>().DeductPoints(selectedUpgrade.GetPrice());
                    selectedItem.LevelUpModule(selectedUpgrade.GetTag());
                    selectedUpgrade.SetInfo(selectedUpgrade.GetTag(), selectedItem);
                }
            }
        }
    }

    private void ListPlayerWeapons()
    {
        if(player && weapons.Count <= 0)
        {
            weapons = player.GetComponent<PlayerInventoryScript>().GetAllWeapons();
            InitializeViewholders();
        }
    }
}
