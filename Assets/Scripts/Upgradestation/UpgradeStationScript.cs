using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeStationScript : MonoBehaviour
{
    [SerializeField]
    private GameObject UI, viewholder;

    [SerializeField]
    private ScrollRect scrollArea;

    [SerializeField]
    private TextMeshProUGUI playerPoints;

    [SerializeField]
    private GameObject descriptionViewholder;

    private Color selectedColor = new Color(1, 0.427451f, 0.0627451f, 1);
    private Color defaultColor = Color.clear;
    private int selectedItemIndex = 0;

    private GameObject player;
    private string imagePath = "Images\\Weapons\\";
    private List<Weapon> weapons = new List<Weapon>();

    private Weapon selectedItem = null;


    private void Start()
    {
        UI.SetActive(false);
        descriptionViewholder.SetActive(false);
    }

    void SetSelected(int index)
    {
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

    private void Update()
    {
        if (player)
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
                player.gameObject.GetComponent<PlayerBuyStationInteraction>().SetInteracting(false);
                player.gameObject.GetComponent<InteractScript>().SetTM("");
                descriptionViewholder.SetActive(false);
                selectedItem = null;
                weapons = new List<Weapon>() { };
                player = null;
                foreach (Transform child in scrollArea.content)
                {
                    if (child != null)
                    {
                        Destroy(child.gameObject);
                    }
                }
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
        descriptionViewholder.SetActive(true);
        //descriptionViewholder.GetComponent<BuyStationDescription>().SetInfo(weapon);
        selectedItem = weapon;
    }

    private void CheckBuyInput()
    {
        if (selectedItem != null && Input.GetKeyDown(KeyCode.E) && player.GetComponent<PlayerBuyStationInteraction>().GetInteracting() && player.GetComponent<PlayerPointsScript>().GetPoints() >= selectedItem.price)
        {
            selectedItem.magazineSize = selectedItem.magazineSize;
            selectedItem.reserveAmmo = selectedItem.startingAmmo;

            if (selectedItem.weaponHolding == WeaponHolding.PRIMARY)
            {
                player.GetComponent<PlayerInventoryScript>().GetPlayerInventory().SetPrimaryWeapon(selectedItem);
            }
            else
            {
                player.GetComponent<PlayerInventoryScript>().GetPlayerInventory().SetSecondaryWeapon(selectedItem);
            }
            player.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sound\\purchase"));
            player.GetComponent<PlayerInventoryScript>().UpdateWeapons();
            player.GetComponent<PlayerPointsScript>().DeductPoints(selectedItem.price);
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
