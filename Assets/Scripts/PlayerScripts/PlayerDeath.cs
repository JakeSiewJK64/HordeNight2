using Cinemachine;
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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        foreach(GameObject go in mainUI)
        {
            go.gameObject.SetActive(false);
        }

        deathScreen.gameObject.SetActive(true);
        Time.timeScale = 0;

        GetComponent<PlayerCameraManager>().StopCamera();
    }

    public void OnQuitButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}