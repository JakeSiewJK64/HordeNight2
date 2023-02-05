using UnityEngine;
using UnityEngine.UIElements;

public class BulletSpawnScript : MonoBehaviour
{
    [SerializeField]
    private Transform bulletSpawn;

    [SerializeField]
    private GameObject bullet;

    private AudioSource audioSource;

    private Transform cameraTransform;
    private float lastBulletSpawn = 0f;
    private float bulletLifetime = 15f;
    private float bulletSpeed = 200f;
    private float shootDelay = .1f;

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
            Vector3 spawnPos = cameraTransform.position + cameraTransform.forward * 5;
            GameObject bulletInstance = Instantiate(bullet, spawnPos, Quaternion.identity);
            Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();
            rb.AddForce(cameraTransform.forward * bulletSpeed, ForceMode.VelocityChange);
            audioSource.PlayOneShot(Resources.Load<AudioClip>(shootSoundPath + "handgun_shoot"));
            lastBulletSpawn = Time.time;
            Destroy(bulletInstance, bulletLifetime);
        }
    }
}
