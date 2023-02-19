using UnityEngine;

public class MaxAmmo : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            ((Weapon)collision.gameObject.GetComponent<PlayerInventoryScript>().GetPlayerInventory().GetSecondaryWeapon()).reserveAmmo =
                ((Weapon)collision.gameObject.GetComponent<PlayerInventoryScript>().GetPlayerInventory().GetSecondaryWeapon()).startingAmmo;

            ((Weapon)collision.gameObject.GetComponent<PlayerInventoryScript>().GetPlayerInventory().GetSecondaryWeapon()).currentBullets =
                ((Weapon)collision.gameObject.GetComponent<PlayerInventoryScript>().GetPlayerInventory().GetSecondaryWeapon()).magazineSize;
        }
    }
}
