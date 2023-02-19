using System.IO;
using UnityEngine;

public class ZombieScript : MonoBehaviour
{
    public Zombie zombie;
    private GameObject player;
    private Animator zombieController;
    private float followRadius = 1000f;

    // player info
    private ZombiesKillCounter counter;
    private PlayerPointsScript playerPoints;
    private ZombieHealthIndicator indicator;

    private string soundPath = "Raw\\Sound\\Zombie\\";

    private void Start()
    {
        zombieController = GetComponentInChildren<Animator>();
        player = GetPlayer();
    }

    private GameObject GetPlayer()
    {
        return player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        float distanceFromTarget = Vector3.Distance(transform.position, player.transform.position);
        if (distanceFromTarget <= followRadius)
        {
            Vector3 targetPosition = player.transform.position;
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                zombie.speed * Time.deltaTime);
            transform.LookAt(player.transform);
        }
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
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            zombie.health -= collision.gameObject.GetComponent<BulletScript>().damage;

            // update zombie health bar info
            counter = collision.gameObject.GetComponent<BulletScript>().GetPlayer().GetComponent<ZombiesKillCounter>();
            playerPoints = collision.gameObject.GetComponent<BulletScript>().GetPlayer().GetComponent<PlayerPointsScript>();

            collision.gameObject.GetComponent<BulletScript>().GetPlayer().GetComponent<ZombieHealthIndicator>().SetZombie(zombie.zombieType.ToString(), zombie.health);
            indicator = collision.gameObject.GetComponent<BulletScript>().GetPlayer().GetComponent<ZombieHealthIndicator>();
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        TakePlayerDamage(collision);
    }
}
