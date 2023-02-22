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

    private GameObject player;
    private string imagePath = "Images\\Weapons\\";
    private bool interacting = false;
    private List<Weapon> weapons = new List<Weapon>();

    private void Start()
    {
        UI.SetActive(false);
        descriptionViewholder.SetActive(false);
        InitializeItems();
        InitializeViewholders();
    }

    private void Update()
    {
        if(player)
        {
            if(Vector3.Distance(transform.position, player.transform.position) > 5)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                UI.SetActive(false);
                interacting = false;
                player.gameObject.GetComponent<InteractScript>()
                    .SetTM("");
                player.gameObject.GetComponent<BulletSpawnScript>().SetInteractingBuyStation(false);
            } else
            {
                if(Input.GetKey(KeyCode.F)) 
                { 
                    UI.SetActive(true);
                    interacting = true;
                    player.gameObject.GetComponent<InteractScript>()
                        .SetTM("");
                    player.gameObject.GetComponent<BulletSpawnScript>().SetInteractingBuyStation(true);
                    playerPoints.text = player.GetComponent<PlayerPointsScript>().GetPoints() + " PTS";
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                }
            }
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
                   damage: 70,
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
                   price: 500)
        };
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
    }
}
