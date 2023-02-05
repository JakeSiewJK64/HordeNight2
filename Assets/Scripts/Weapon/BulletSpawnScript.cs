using UnityEngine;

public class BulletSpawnScript : MonoBehaviour
{
    [SerializeField]
    private Transform bulletSpawn;

    [SerializeField]
    private GameObject bullet;

    private AudioSource audioSource;

    private Transform cameraTransform;
    private float lastBulletSpawn = 0f;
    private float bulletLifetime = 5f;
    private float bulletSpeed = 500f;
    private float shootDelay = .2f;

    private string shootSoundPath = "Sound\\WeaponSounds\\";

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(Input.GetMouseButton(0) && Time.time - lastBulletSpawn > shootDelay)
        {
            GameObject bulletInstance = Instantiate(bullet, bulletSpawn);
            Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();
            rb.AddForce(cameraTransform.forward * bulletSpeed, ForceMode.VelocityChange);
            audioSource.PlayOneShot(Resources.Load<AudioClip>(shootSoundPath + "handgun_shoot"));
            lastBulletSpawn = Time.time;
            Destroy(bulletInstance, bulletLifetime);
        }
    }
}
