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
    private float bulletSpeed = 200f;
    private float shootDelay = .1f;

    private string shootSoundPath = "Sound\\WeaponSounds\\";

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        audioSource = GetComponent<AudioSource>();
    }

    private GameObject SpawnBullet(GameObject bullet, Vector3 pos, Vector3 direction)
    {
        GameObject bulletInstance = Instantiate(bullet, pos, Quaternion.identity);
        Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();
        rb.AddForce(direction * bulletSpeed, ForceMode.VelocityChange);        
        Destroy(bulletInstance, bulletLifetime);
        lastBulletSpawn = Time.time;
        audioSource.PlayOneShot(Resources.Load<AudioClip>(shootSoundPath + "handgun_shoot"));
        return bulletInstance;
    }

    void Update()
    {
        if(Input.GetMouseButton(0) && Time.time - lastBulletSpawn > shootDelay)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 direction = (ray.origin + ray.direction * 100) - bulletSpawn.position;
            GameObject bulletFromCamera = SpawnBullet(bullet, cameraTransform.position + cameraTransform.forward * 5, direction.normalized);
            bulletFromCamera.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
