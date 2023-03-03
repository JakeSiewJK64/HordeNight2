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
    private float circularSpread = 1f, bulletSpeed = 1f;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        currentWeapon = gameObject.GetComponent<PlayerInventoryScript>().GetCurrentWeapon();
        audioSource = GetComponent<AudioSource>();
        reloadSound = Resources.Load<AudioClip>(Path.Combine(soundFolder, currentWeapon.reloadingSoundPath));
        UpdateWeaponSound();
    }

    public void ChangeWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
        reloading = false;
        UpdateWeaponSound();
    }

    private void UpdateWeaponSound()
    {
        if(currentWeapon != null)
        {
            shootingSound = Resources.Load<AudioClip>(Path.Combine(soundFolder, currentWeapon.shootingSoundPath));
            reloadSound = Resources.Load<AudioClip>(Path.Combine(soundFolder, currentWeapon.reloadingSoundPath));
        }
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
        if (currentWeapon != null && Time.time - lastClickTime > currentWeapon.reloadTime)
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
        if(currentWeapon != null)
        {
            bulletCounterIndicator.text = currentWeapon.currentBullets + "\n" + currentWeapon.reserveAmmo;
        }
    }

    private GameObject SpawnBullet(Vector3 direction, Quaternion rotation)
    {
        GameObject bulletInstance = Instantiate(bulletPrefab, bulletSpawn.position, rotation);
        bulletInstance.GetComponent<Rigidbody>().AddForce(direction.normalized * bulletSpeed);
        Destroy(bulletInstance, bulletLifetime);
        lastClickTime = Time.time;
        bulletInstance.GetComponent<BulletScript>().SetOriginPlayer(gameObject);
        return bulletInstance;
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0) && !reloading)
        {
            // todo: factor in fire rate upgrade
            //if (Time.time - lastClickTime > currentWeapon.fireRate - (currentWeapon.upgradeStats.fireRate.GetValue() / 100) * currentWeapon.fireRate)
            if (currentWeapon != null && Time.time - lastClickTime > currentWeapon.fireRate)
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
                    SpawnBullet(direction, Quaternion.identity);
                }
                else
                {
                    for (int i = 0; i < 12; i++)
                    {
                        Vector3 spread = Vector3.zero;
                        spread += cameraTransform.transform.up * Random.Range(-circularSpread, circularSpread); // add random up or down (because random can get negative too)
                        spread += cameraTransform.transform.right * Random.Range(-circularSpread, circularSpread); // add random left or right

                        // Using random up and right values will lead to a square spray pattern. If we normalize this vector, we'll get the spread direction, but as a circle.
                        // Since the radius is always 1 then (after normalization), we need another random call. 

                        direction += spread;

                        SpawnBullet(direction, Quaternion.identity);
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (Time.timeScale == 1.0f && !GetComponent<PlayerBuyStationInteraction>().GetInteracting())
        {
            UpdateBulletCount();
            checkReloading();
            if (currentWeapon != null && currentWeapon.currentBullets == 0)
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
