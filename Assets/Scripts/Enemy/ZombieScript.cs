using System.IO;
using UnityEngine;

public class ZombieScript : MonoBehaviour
{
    public Zombie zombie;

    private string soundPath = "Raw\\Sound\\Zombie\\";
    private Animator zombieController;

    // player info
    private ZombiesKillCounter counter;
    private PlayerPointsScript playerPoints;
    private ZombieHealthIndicator indicator;

    private void Start()
    {
        zombieController = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        CheckHealth();
    }

    private void TakePlayerDamage(Collision collision)
    {
        try
        {
            if (collision.gameObject.tag == "Player" && collision != null && gameObject != null)
            {
                zombieController.SetBool("Attacking", true);
                collision.gameObject.GetComponent<PlayerHealthbar>().TakeDamage(zombie.damage);

                if (!GetComponent<AudioSource>().isPlaying)
                {
                    string voice = Random.Range(0, 5) == 0 ? "zombie_quick" : "zombie_long";
                    GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>(Path.Combine(soundPath, voice)));
                }

                return;
            }
            else
            {
                zombieController.SetBool("Attacking", false);
            }
        }
        catch { }
    }

    void CheckHealth()
    {
        if (zombie.health <= 0)
        {
            counter.IncrementCounter();
            playerPoints.IncrementPoints(100f);
            indicator.HideUI();
            Destroy(transform.parent.gameObject);
        }
    }

    public void SetPlayerInfo(Collision collision)
    {
        counter = collision.gameObject.GetComponent<BulletScript>().GetPlayer().GetComponent<ZombiesKillCounter>();
        playerPoints = collision.gameObject.GetComponent<BulletScript>().GetPlayer().GetComponent<PlayerPointsScript>();
        indicator = collision.gameObject.GetComponent<BulletScript>().GetPlayer().GetComponent<ZombieHealthIndicator>();
        collision.gameObject.GetComponent<BulletScript>().GetPlayer().GetComponent<ZombieHealthIndicator>().SetZombie(zombie.zombieType.ToString(), zombie.health);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            zombie.health -= collision.gameObject.GetComponent<BulletScript>().damage;
            collision.gameObject.GetComponent<BulletScript>().GetPlayer().GetComponent<HitMarker>().UpdateCursor(Hitmarker.hitmarker);
            SetPlayerInfo(collision);
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        TakePlayerDamage(collision);
    }
}
