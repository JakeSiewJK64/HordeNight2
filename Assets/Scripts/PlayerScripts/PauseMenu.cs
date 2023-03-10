using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    private bool paused = false;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !GetComponent<PlayerBuyStationInteraction>().GetInteracting())
        {
            if(GetComponent<PlayerHealthbar>().player.health > 0)
            {
                if(paused)
                {
                    OnUnPauseButtonPressed();
                    return;
                }
                paused = true;
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                GetComponent<PlayerCameraManager>().StopCamera();
            }
        }
    }

    public void OnUnPauseButtonPressed()
    {
        paused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GetComponent<PlayerCameraManager>().ResumeCamera();
    }

    public void OnExitButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
