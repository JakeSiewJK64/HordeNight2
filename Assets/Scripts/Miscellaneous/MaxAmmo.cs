using UnityEngine;

public class MaxAmmo : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            foreach(var weapon in collision.gameObject.GetComponent<PlayerInventoryScript>().GetAllWeapons())
            {
                weapon.currentBullets = weapon.magazineSize;
                weapon.reserveAmmo = weapon.startingAmmo;
            }
        }
    }
}
