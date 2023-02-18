using System.IO;
using TMPro;
using UnityEngine;

public class BulletSpawnScript : MonoBehaviour
{
    private string soundFolder = "Sound\\WeaponSounds";

    private Transform cameraTransform;

    [SerializeField]
    private TextMeshProUGUI bulletCounterIndicator;

    // for bullet casing effect
    public GameObject bulletCasingPrefab;
    public Transform bulletCasingSpawn;

    // actual weapon effect
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletLifetime = 2f;
    public float lastClickTime;

    private Weapon currentWeapon;
    private AudioSource audioSource;

    private AudioClip reloadSound;
    private AudioClip shootingSound;

    public bool reloading = false;

    public Inventory inventory;

    [SerializeField]
    float spreadAngle = .5f;

    public void ChangeWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
        reloading = false;
        UpdateWeaponSound();
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        currentWeapon = gameObject.GetComponent<PlayerInventoryScript>().GetCurrentWeapon();

        // initialize audio
        audioSource = GetComponent<AudioSource>();
        reloadSound = Resources.Load<AudioClip>(Path.Combine(soundFolder, currentWeapon.reloadingSoundPath));
        UpdateWeaponSound();
    }

    private void UpdateWeaponSound()
    {
        shootingSound = Resources.Load<AudioClip>(Path.Combine(soundFolder, currentWeapon.shootingSoundPath));
        reloadSound = Resources.Load<AudioClip>(Path.Combine(soundFolder, currentWeapon.reloadingSoundPath));
    }

    private void PlayWeaponSound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    private void checkReloading()
    {
        if (Input.GetKeyDown(KeyCode.R) && !reloading && currentWeapon.currentBullets != currentWeapon.GetMagazineSize())
        {
            Reload();
        }

        // todo: factor in reload speed upgrade stats
        if (Time.time - lastClickTime > currentWeapon.reloadTime)
        {
            reloading = false;
        }
    }

    private void Reload()
    {
        if (currentWeapon.reserveAmmo > 0)
        {
            reloading = true;
            currentWeapon.Reload();
            PlayWeaponSound(reloadSound);
        }
    }

    private void UpdateBulletCount()
    {
        bulletCounterIndicator.text = currentWeapon.currentBullets + "\n" + currentWeapon.reserveAmmo;
    }

    private void SpawnBulletCasing(Rigidbody rb)
    {
        // trigger bullet casing effect
        GameObject bulletCasing = Instantiate(bulletCasingPrefab, bulletCasingSpawn.position, bulletCasingSpawn.rotation);
        rb.AddForce(bulletCasingPrefab.transform.forward);
        Destroy(bulletCasing, bulletLifetime);
    }

    private GameObject SpawnBullet(GameObject bullet, Vector3 pos, Vector3 direction, string type)
    {
        Quaternion originalRotation = bullet.transform.rotation;
        Quaternion rotationBy90 = Quaternion.Euler(0, 0, 90);
        GameObject bulletInstance = Instantiate(bullet, pos, originalRotation * rotationBy90);
        Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();
        rb.AddForce(direction * 100f, ForceMode.VelocityChange);
        Destroy(bulletInstance, bulletLifetime);
        lastClickTime = Time.time;
        return bulletInstance;
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0) && !reloading)
        {
            // todo: factor in fire rate upgrade
            //if (Time.time - lastClickTime > currentWeapon.fireRate - (currentWeapon.upgradeStats.fireRate.GetValue() / 100) * currentWeapon.fireRate)
            if (Time.time - lastClickTime > currentWeapon.fireRate)
            {
                currentWeapon.currentBullets--;
                lastClickTime = Time.time;

                // todo: factor in upgrade stats
                //bulletPrefab.GetComponent<BulletScript>().damage =
                //    (float)(currentWeapon.damage + (currentWeapon.damage * (int)currentWeapon.weaponType * currentWeapon.upgradeStats.damage.GetValue()));
                
                bulletPrefab.GetComponent<BulletScript>().damage = currentWeapon.damage;

                // play shoot sound
                PlayWeaponSound(shootingSound);

                GameObject bullet;
                Rigidbody rb;
                if (currentWeapon.weaponType != WeaponType.Shotgun)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Vector3 direction = (ray.origin + ray.direction * 100) - bulletSpawn.position;
                    GameObject bulletFromCamera = SpawnBullet(bulletPrefab, cameraTransform.position + cameraTransform.forward * 5, direction.normalized, "Bullet");
                    bulletFromCamera.GetComponent<MeshRenderer>().enabled = false;
                    bulletFromCamera.GetComponent<BulletScript>().SetOriginPlayer(gameObject);

                    // todo: spawn bullet trail
                    // SpawnBullet(bulletTrail, bulletSpawn.position + cameraTransform.forward * 5, direction.normalized, "BulletTrail");
                }
                else
                {
                    for (int i = 0; i < 12; i++)
                    {
                        bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
                        bullet.GetComponent<BulletScript>().SetOriginPlayer(gameObject);

                        // setting the pellet spread
                        Vector3 spread = new Vector3(Random.Range(-spreadAngle, spreadAngle), Random.Range(-spreadAngle, spreadAngle), Random.Range(-spreadAngle, spreadAngle));

                        bullet.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + spread);
                        rb = bullet.GetComponent<Rigidbody>();
                        rb.AddForce(transform.forward * 50);
                        Destroy(bullet, bulletLifetime);
                    }
                }
            }
        }
    }

    private void Update()
    {
        // todo: if interacting with buy station dont shoot
        //if (!GetComponent<BuyStationScript>().interacting && Time.timeScale == 1.0f)
        if (Time.timeScale == 1.0f)
        {
            UpdateBulletCount();
            checkReloading();
            if (currentWeapon.currentBullets == 0)
            {
                Reload();
            }
            else
            {
                Shoot();
            }
        }
    }
}
