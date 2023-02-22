using UnityEngine;

public class PlayerBuyStationInteraction : MonoBehaviour
{
    [SerializeField]
    private Cinemachine.CinemachineVirtualCameraBase vcam;

    [SerializeField]
    private int boost = 10;

    private bool interacting = false;

    public bool GetInteracting()
    {
        return interacting;
    }

    public void SetInteracting(bool interacting)
    {
        this.interacting = interacting;
        BoostCamera();
    }

    public void BoostCamera()
    {
        if(interacting)
        {
            vcam.Priority += boost;
            return;
        } 
        vcam.Priority = 0;
    }
}
