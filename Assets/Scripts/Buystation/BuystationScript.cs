using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuystationScript : MonoBehaviour
{
    [SerializeField]
    private GameObject UI, viewholder;

    [SerializeField]
    private ScrollRect scrollArea;

    [SerializeField]
    private TextMeshProUGUI playerPoints;

    [SerializeField]
    private GameObject descriptionViewholder;

    [SerializeField]
    private Transform platform;
    
    private Color selectedColor = Color.red;
    private Color defaultColor = Color.clear;
    private int selectedItemIndex = 0;

    private GameObject player;
    private string imagePath = "Images\\Weapons\\";
    private bool interacting = false;
    private List<Weapon> weapons = new List<Weapon>();

    private Weapon selectedItem = null;


    private void Start()
    {
        UI.SetActive(false);
        descriptionViewholder.SetActive(false);
        InitializeItems();
        InitializeViewholders();
    }

    void SetSelected(int index)
    {
        for (int i = 0; i < scrollArea.content.childCount; i++)
        {
            if (i == index)
            {
                scrollArea.content.GetChild(i).GetComponent<BuyStationViewholder>().SetSelected(true);
                scrollArea.content.GetChild(i).GetComponent<Image>().color = selectedColor;
            }
            else
            {
                scrollArea.content.GetChild(i).GetComponent<BuyStationViewholder>().SetSelected(false);
                scrollArea.content.GetChild(i).GetComponent<Image>().color = defaultColor;
            }
        }
    }

    private void Update()
    {
        if(player)
        {
            CheckBuyInput();

            // todo: detect arrow keys
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                selectedItemIndex = Mathf.Min(selectedItemIndex + 1, scrollArea.content.childCount - 1);
                SetSelected(selectedItemIndex);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                selectedItemIndex = Mathf.Max(selectedItemIndex - 1, 0);
                SetSelected(selectedItemIndex);
            }

            // scroll based on index
            int index = selectedItemIndex;
            float normalizedPosition = (float)index / (scrollArea.content.childCount - 1);
            scrollArea.normalizedPosition = new Vector2(0, 1 - normalizedPosition);

            if (Vector3.Distance(transform.position, player.transform.position) > 5)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                UI.SetActive(false);
                interacting = false;
                player.gameObject.GetComponent<PlayerBuyStationInteraction>().SetInteracting(false);
                descriptionViewholder.SetActive(false);
                player.gameObject.GetComponent<InteractScript>().SetTM("");
            } else
            {
                if(Input.GetKey(KeyCode.F)) 
                { 
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    UI.SetActive(true);
                    interacting = true;
                    player.gameObject.GetComponent<PlayerBuyStationInteraction>().SetInteracting(true);
                    playerPoints.text = player.GetComponent<PlayerPointsScript>().GetPoints() + " PTS";
                    player.gameObject.GetComponent<InteractScript>().SetTM("");
                }

            }
        }
    }

    private void InitializeViewholders()
    {
        foreach(var item in weapons)
        {
            GameObject newItem = Instantiate(viewholder, scrollArea.content);
            if (newItem.TryGetComponent(out BuyStationViewholder viewholderItem))
            {
                viewholderItem.SetInfo(new ViewholderItems(
                    name: item.name,
                    damage: item.damage.ToString(),
                    rateOfFire: item.fireRate.ToString(),
                    reloadSpeed: item.reloadTime.ToString(),
                    magazineSize: item.magazineSize.ToString(),
                    points: item.price.ToString(),
                    weaponProfile: Resources.Load<Sprite>(Path.Combine(imagePath, item.weaponIconPath.ToString())),
                    weapon: item));
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            if (!interacting)
            {
                collision
                    .gameObject
                    .GetComponent<InteractScript>()
                    .SetTM("[ F ] to interact with buy station");
            }
        }
    }

    public void UpdateDescription(Weapon weapon)
    {
        descriptionViewholder.SetActive(true);
        descriptionViewholder.GetComponent<BuyStationDescription>().SetInfo(weapon);
        selectedItem = weapon;
    }

    private void CheckBuyInput()
    {
        if(selectedItem != null && Input.GetKeyDown(KeyCode.E) && interacting && player.GetComponent<PlayerPointsScript>().GetPoints() >= selectedItem.price)
        {
            if (selectedItem.weaponHolding == WeaponHolding.PRIMARY)
            {
                player.GetComponent<PlayerInventoryScript>().GetPlayerInventory().SetPrimaryWeapon(selectedItem);
            } else
            {
                player.GetComponent<PlayerInventoryScript>().GetPlayerInventory().SetSecondaryWeapon(selectedItem);
            }
            player.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sound\\purchase"));
            player.GetComponent<PlayerInventoryScript>().UpdateWeapons();
            player.GetComponent<PlayerPointsScript>().DeductPoints(selectedItem.price);
        }
    }

    private void InitializeItems()
    {
        weapons = new List<Weapon>
        {
            new Weapon("m4", "description", ItemType.Weapon, WeaponType.AssaultRifle, WeaponHolding.PRIMARY,
                    reserveAmmo: 10000,
                    startingAmmo: 10000,
                    damage: 70,
                    magazineSize: 30,
                    currentBullets: 30,
                    fireRate: .1f,
                    reloadTime: 1f,
                    shootingSoundPath: "handgun_shoot",
                    reloadingSoundPath: "handgun_reload",
                    weaponIconPath: "m4",
                    weaponPrefabPath: "glock18",
                    price: 1500),
            new Weapon("shotgun", "description", ItemType.Weapon, WeaponType.Shotgun, WeaponHolding.PRIMARY,
                   reserveAmmo: 40,
                   startingAmmo: 40,
                   damage: 80,
                   magazineSize: 8,
                   currentBullets: 8,
                   fireRate: .2f,
                   reloadTime: 2f,
                   shootingSoundPath: "handgun_shoot",
                   reloadingSoundPath: "handgun_reload",
                   weaponIconPath: "shotgun_1",
                   weaponPrefabPath: "glock18",
                   price: 3500),
            new Weapon("glock 18", "description", ItemType.Weapon, WeaponType.Sidearm, WeaponHolding.SECONDARY,
                   reserveAmmo: 40,
                   startingAmmo: 40,
                   damage: 70,
                   magazineSize: 8,
                   currentBullets: 8,
                   fireRate: .2f,
                   reloadTime: 2f,
                   shootingSoundPath: "handgun_shoot",
                   reloadingSoundPath: "handgun_reload",
                   weaponIconPath: "glock",
                   weaponPrefabPath: "glock18",
                   price: 500),
            new Weapon("Punisher", "description", ItemType.Weapon, WeaponType.Sidearm, WeaponHolding.SECONDARY,
                   reserveAmmo: 40,
                   startingAmmo: 40,
                   damage: 75,
                   magazineSize: 12,
                   currentBullets: 8,
                   fireRate: .15f,
                   reloadTime: 2f,
                   shootingSoundPath: "handgun_shoot",
                   reloadingSoundPath: "handgun_reload",
                   weaponIconPath: "glock",
                   weaponPrefabPath: "glock18",
                   price: 500),
            new Weapon("origin 12 Shotgun", "description", ItemType.Weapon, WeaponType.Shotgun, WeaponHolding.PRIMARY,
                   reserveAmmo: 40,
                   startingAmmo: 40,
                   damage: 80,
                   magazineSize: 8,
                   currentBullets: 8,
                   fireRate: .1f,
                   reloadTime: 2f,
                   shootingSoundPath: "handgun_shoot",
                   reloadingSoundPath: "handgun_reload",
                   weaponIconPath: "shotgun_1",
                   weaponPrefabPath: "glock18",
                   price: 3500),
        };
    }
}
