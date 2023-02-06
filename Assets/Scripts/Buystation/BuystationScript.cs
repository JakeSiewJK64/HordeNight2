using UnityEngine;

public class BuystationScript : MonoBehaviour
{
    [SerializeField]
    private GameObject UI;
    private GameObject player;

    private void Start()
    {
        UI.SetActive(false);
    }

    private void Update()
    {
        if(player)
        {
            if(Vector3.Distance(transform.position, player.transform.position) > 5)
            {
                UI.SetActive(false);
                player.gameObject.GetComponent<InteractScript>()
                    .SetTM("");
            } else
            {
                if(Input.GetKey(KeyCode.F)) 
                { 
                    UI.SetActive(true);
                    player.gameObject.GetComponent<InteractScript>()
                        .SetTM("");
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            collision
                .gameObject
                .GetComponent<InteractScript>()
                .SetTM("[ F ] to interact with buy station");
        }
    }
}
