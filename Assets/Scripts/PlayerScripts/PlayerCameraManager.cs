using Cinemachine;
using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera[] cameras;

    public void StopCamera()
    {
        foreach (var item in cameras)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void ResumeCamera()
    {
        foreach (var item in cameras)
        {
            item.gameObject.SetActive(true);
        }
    }
}
