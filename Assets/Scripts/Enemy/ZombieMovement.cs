using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    private GameObject player;
    private float followRadius = 1000f;

    private void Start()
    {
        player = GetPlayer();
    }

    private GameObject GetPlayer()
    {
        return player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        try
        {
            float distanceFromTarget = Vector3.Distance(transform.position, player.transform.position);
            if (distanceFromTarget <= followRadius)
            {
                Vector3 targetPosition = player.transform.position;
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    GetComponentInChildren<ZombieScript>().zombie.speed * Time.deltaTime);
                transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            }
        }
        catch { }
    }
}
