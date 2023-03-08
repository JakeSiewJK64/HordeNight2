using System.IO;
using UnityEngine;

public class ZombieScript : MonoBehaviour
{
    [SerializeField]
    private Transform RagdollRoot;

    public Zombie zombie;

    private string soundPath = "Raw\\Sound\\Zombie\\";
    private Animator zombieController;

    // ragdoll variables
    private Rigidbody[] rigidbodies;
    private CharacterJoint[] joints;
    private Collider[] colliders;

    // player info
    private ZombiesKillCounter counter;
    private PlayerPointsScript playerPoints;
    private ZombieHealthIndicator indicator;

    public bool dead = false;
    public float deadTime = 0;

    private void Start()
    {
        zombieController = GetComponentInChildren<Animator>();
        rigidbodies = RagdollRoot.GetComponentsInChildren<Rigidbody>();
        joints = RagdollRoot.GetComponentsInChildren<CharacterJoint>();
        colliders = RagdollRoot.GetComponentsInChildren<Collider>();

        EnableAnimator();
    }

    private void EnableAnimator()
    {
        zombieController.enabled = true;
        foreach(CharacterJoint joint in joints)
        {
            joint.enableCollision = false;
        }

        foreach(Collider collider in colliders)
        {
            collider.enabled = false;
        }

        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;
        }
    }

    private void EnableRagdoll()
    {
        zombieController.enabled = false;
        foreach (CharacterJoint joint in joints)
        {
            joint.enableCollision = true;
        }

        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
        }
    }

    private void Update()
    {
        CheckHealth();
    }

    private void TakePlayerDamage(Collision collision)
    {
        try
        {
            if (collision.gameObject.tag == "Player" && collision != null && gameObject != null && !dead)
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
        if(dead && Time.time - deadTime > 10f)
        {
            Destroy(transform.parent.gameObject);
        }

        if (zombie.health <= 0 && !dead)
        {
            counter.IncrementCounter();
            playerPoints.IncrementPoints(100f);
            indicator.HideUI();
            EnableRagdoll();
            dead = true;
            deadTime = Time.time;
            gameObject.tag = "Untagged";
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
