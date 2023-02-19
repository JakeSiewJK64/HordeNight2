using UnityEngine;

public class HeadshotMultiplier : MonoBehaviour
{
    [SerializeField]
    private GameObject zombieBody;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            zombieBody.GetComponentInParent<ZombieScript>().zombie.health -= collision.gameObject.GetComponent<BulletScript>().damage * 2;
        }
    }
}