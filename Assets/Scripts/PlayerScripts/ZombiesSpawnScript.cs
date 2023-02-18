using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZombiesSpawnScript : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public List<GameObject> spawnPads;

    public float spawnRadius = 1000f;
    public float restrictedSpawnRadius = 10f;
    public float minDistanceFromPlayer = 20f;

    public int spawnYAxis = 4;

    public int remainingZombies;

    public TextMeshProUGUI zombieCounter;
    public TextMeshProUGUI roundCounter;

    public Vector3 spawnPos;

    private float lastZombieSpawnTime;

    private float spawnDelay = 1f;
    private void Checkspawnpad()
    {
        spawnPads = new List<GameObject>();
        // Get all GameObjects in radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, spawnRadius);

        // Check each GameObject for the "Spawnpad" tag
        foreach (Collider col in hitColliders)
        {
            // if (col.CompareTag("Spawnpad") && Vector3.Distance(col.transform.position, transform.position) > restrictedSpawnRadius)
            if (col.CompareTag("Spawnpad"))
            {
                spawnPads.Add(col.gameObject);
            }
        }
    }

    private void SpawnZombies()
    {
        if (Time.time - lastZombieSpawnTime >= spawnDelay)
        {
            Checkspawnpad();
            GameObject spawnPad = spawnPads[Random.Range(0, spawnPads.Count - 1)];
            spawnPos = new Vector3(spawnPad.transform.position.x, spawnPad.transform.position.y + 3, spawnPad.transform.position.z);
            GameObject zombie = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
            zombie.GetComponent<ZombieScript>().zombie = new Zombie(
                zombieType: ZombieType.Zombie,
                speed: Random.Range(2, 5),
                health: 100 + 100 * (GetComponent<ZombiesKillCounter>().GetRound() / GetComponent<ZombiesKillCounter>().GetBloodMoon()),
                damage: 1
            );
            lastZombieSpawnTime = Time.time;
        }
    }

    private void CheckZombies()
    {
        remainingZombies = GameObject.FindGameObjectsWithTag("Zombie").Length;

        zombieCounter.text = "Remaining Zombies: " + remainingZombies;
        roundCounter.text = "Round: " + GetComponent<ZombiesKillCounter>().GetRound();

        int zombiesKilled = GetComponent<ZombiesKillCounter>().GetZombiesKilled();
        int maxZombies = GetComponent<ZombiesKillCounter>().GetMaxZombies();

        if (remainingZombies <= maxZombies && remainingZombies + zombiesKilled == maxZombies)
        {
            if (remainingZombies == 0)
            {
                GetComponent<ZombiesKillCounter>().ChangeRound();
            }
        }
        else
        {
            SpawnZombies();
        }
    }

    private void Update()
    {
        CheckZombies();
    }
}
