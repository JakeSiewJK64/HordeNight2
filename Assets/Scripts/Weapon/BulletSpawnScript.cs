using UnityEngine;

public class BulletSpawnScript : MonoBehaviour
{
    [SerializeField]
    private Transform bulletSpawn;

    [SerializeField]
    private GameObject bullet, bulletTrail;

    private AudioSource audioSource;

    private Transform cameraTransform;
    private float lastBulletSpawn = 0f;
    private float bulletLifetime = 3f;
    private float bulletSpeed = 200f;
    private float shootDelay = .1f;
    private string shootSoundPath = "Sound\\WeaponSounds\\";

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        audioSource = GetComponent<AudioSource>();
    }

    private GameObject SpawnBullet(GameObject bullet, Vector3 pos, Vector3 direction, string type)
    {
        Quaternion originalRotation = bullet.transform.rotation;
        Quaternion rotationBy90 = Quaternion.Euler(0, 0, 90);

        GameObject bulletInstance = Instantiate(bullet, pos, originalRotation * rotationBy90);
        Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();
        rb.AddForce(direction * (type == "Bullet" ? bulletSpeed : 1000f), ForceMode.VelocityChange);
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
            GameObject bulletFromCamera = SpawnBullet(bullet, cameraTransform.position + cameraTransform.forward * 5, direction.normalized, "Bullet");
            bulletFromCamera.GetComponent<MeshRenderer>().enabled = false;
            SpawnBullet(bulletTrail, bulletSpawn.position + cameraTransform.forward * 5, direction.normalized, "BulletTrail");
        }
    }
}
