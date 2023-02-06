using TMPro;
using UnityEngine;

public class InteractScript : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI tm;

    private void Start()
    {
        tm.gameObject.SetActive(false);
    }

    public void SetTM(string message)
    {
        tm.text = message;
        tm.gameObject.SetActive(true);
    }
}
