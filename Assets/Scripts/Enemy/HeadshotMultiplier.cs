using UnityEngine;

public class HeadshotMultiplier : MonoBehaviour
{
    [SerializeField]
    private GameObject zombieBody, zombieHead;

    private void Update()
    {
        transform.position = zombieHead.transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            zombieBody.GetComponentInChildren<ZombieScript>().zombie.health -= collision.gameObject.GetComponent<BulletScript>().damage * 2;
            zombieBody.GetComponentInChildren<ZombieScript>().SetPlayerInfo(collision);
            collision.gameObject.GetComponent<BulletScript>().GetPlayer().GetComponent<HitMarker>().UpdateCursor(Hitmarker.headshot);
            Destroy(collision.gameObject);
        }
    }
}