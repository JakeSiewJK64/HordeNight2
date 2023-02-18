using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField]
    private GameObject[] mainUI;

    [SerializeField]
    private GameObject deathScreen;

    private void Start()
    {
        deathScreen.gameObject.SetActive(false);
    }

    public void PromptDeathScreen()
    {
        // manage buy station UI
        //GetComponent<BuyStationScript>().CloseBuyStation();

        //GetComponent<PlayerHealthScript>().player.dead = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        foreach(GameObject go in mainUI)
        {
            go.gameObject.SetActive(false);
        }

        deathScreen.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void OnQuitButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}