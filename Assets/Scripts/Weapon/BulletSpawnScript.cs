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
    private float spreadValue = 2f;

    private float bulletSpeed = 1f;

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

    private void SpawnBullet(Vector3 direction)
    {
        GameObject bulletInstance = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        bulletInstance.GetComponent<Rigidbody>().AddForce(direction.normalized * bulletSpeed);
        Destroy(bulletInstance, bulletLifetime);
        lastClickTime = Time.time;
        bulletInstance.GetComponent<BulletScript>().SetOriginPlayer(gameObject);
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
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 direction = (ray.origin + ray.direction * 100) - bulletSpawn.position;

                if (currentWeapon.weaponType != WeaponType.Shotgun)
                {
                    SpawnBullet(direction);
                }
                else
                {
                    for (int i = 0; i < 12; i++)
                    {
                        // Calculate the spread angle
                        float spreadAngle = Random.Range(-spreadValue, spreadValue);
                        // Rotate the direction vector by the spread angle
                        direction = Quaternion.Euler(spreadAngle / 2, spreadAngle, 0) * direction;
                        SpawnBullet(direction);
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
