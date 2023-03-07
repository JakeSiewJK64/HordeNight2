using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerkStationScript : MonoBehaviour
{
    [SerializeField]
    private GameObject UI, viewholder;

    [SerializeField]
    private TextMeshProUGUI playerPoints;

    [SerializeField]
    private ScrollRect scrollArea;

    private Color selectedColor = new Color(1, 0.427451f, 0.0627451f, 1);
    private Color defaultColor = Color.clear;

    private GameObject player;

    private int globalIndex = 0;

    private void Start()
    {
        UI.SetActive(false);
    }

    private void Update()
    {
        if (player)
        {
            CheckPlayerInteraction();
        }

        ControlScrollArea(scrollArea);

        CheckBuyInput();
    }

    void SetSelected(int index, ScrollRect scrollArea)
    {
        for (int i = 0; i < scrollArea.content.childCount; i++)
        {
            if (i == index)
            {
                scrollArea.content.GetChild(i).GetComponent<PerkStationViewholder>().SetSelected(true);
                scrollArea.content.GetChild(i).GetComponent<Image>().color = selectedColor;
            }
            else
            {
                scrollArea.content.GetChild(i).GetComponent<PerkStationViewholder>().SetSelected(false);
                scrollArea.content.GetChild(i).GetComponent<Image>().color = defaultColor;
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
                ListPerkUpgrades();
                player.gameObject.GetComponent<PlayerBuyStationInteraction>().SetInteracting(true);
                playerPoints.text = player.GetComponent<PlayerPointsScript>().GetPoints() + " PTS";
                player.gameObject.GetComponent<InteractScript>().SetTM("");
            }
        }
    }

    private void ControlScrollArea(ScrollRect scrollArea)
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            globalIndex = Mathf.Min(globalIndex + 1, scrollArea.content.childCount - 1);
            SetSelected(globalIndex, scrollArea);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            globalIndex = Mathf.Max(globalIndex - 1, 0);
            SetSelected(globalIndex, scrollArea);
        }

        // scroll based on index
        int index = globalIndex;
        float normalizedPosition = (float)index / (scrollArea.content.childCount - 1);
        scrollArea.normalizedPosition = new Vector2(0, 1 - normalizedPosition);
    }

    private void CheckBuyInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && player.GetComponent<PlayerBuyStationInteraction>().GetInteracting())
        {
            player.GetComponent<PlayerHealthbar>().player.LevelUpModule(scrollArea.content.GetChild(globalIndex).GetComponent<PerkStationViewholder>().GetUpgrade().Key);
            scrollArea.content.GetChild(globalIndex).GetComponent<PerkStationViewholder>().SetInfo
                (
                    scrollArea.content.GetChild(globalIndex).GetComponent<PerkStationViewholder>().GetUpgrade(),
                    player.GetComponent<PlayerHealthbar>().player
                );
        }
    }

    private void ClearScrollContent()
    {
        foreach (Transform child in scrollArea.content)
        {
            if (child != null)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void ListPerkUpgrades()
    {
        Player playerClass = player.GetComponent<PlayerHealthbar>().player;
        
        // todo: enumerate available perk upgrade
        if (player && scrollArea.content.childCount <= 0)
        {
            foreach (var perk in playerClass.upgradeModule)
            {
                GameObject newItem = Instantiate(viewholder, scrollArea.content);
                if (newItem.TryGetComponent(out PerkStationViewholder viewholderItem))
                {
                    viewholderItem.SetInfo(perk, playerClass);
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
}
