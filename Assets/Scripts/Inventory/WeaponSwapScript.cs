using UnityEngine;
using UnityEngine.UI;

public class WeaponSwapScript : MonoBehaviour
{
    [SerializeField]
    private GameObject weaponSlotUI;

    [SerializeField]
    private Image[] weaponSlots;
    private string path = "Images\\";
    private string[] weaponNames = { "Weapon1", "Weapon2", "Weapon3" };
    private float lastToggle = 0f;
    private float weaponSlotDuration = 2f;

    private void Start()
    {
        weaponSlotUI.gameObject.SetActive(false);
        UpdateWeaponUI(weaponNames[0]);
    }

    private void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        for (int i = 0; i < weaponNames.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                weaponSlotUI.gameObject.SetActive(true);
                lastToggle = Time.time;
                UpdateWeaponUI(weaponNames[i]);
                break;
            }
        }

        if(Time.time - lastToggle > weaponSlotDuration)
        {
            weaponSlotUI.gameObject.SetActive(false);
        }
    }

    private void UpdateWeaponUI(string name)
    {
        foreach(Image image in weaponSlots)
        {
            foreach(Image subimage in image.GetComponentsInChildren<Image>())
            {
                if(subimage.tag == "Select")
                {
                    subimage.color = 
                        image.name == name ? new Color(1f, 1f, 1f, 1f) 
                            : subimage.color = new Color(1f, 1f, 1f, 0f);
                }
            }
        }
    }
}
