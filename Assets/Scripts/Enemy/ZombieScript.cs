using UnityEngine;

public class ZombieScript : MonoBehaviour
{
    private GameObject player;
    private float followRadius = 1000f;
    private bool attacking = false;

    private GameObject GetPlayer()
    {
        return GameObject.FindWithTag("Player");
    }

    private void Start()
    {
        player = GetPlayer();
    }

    private void MoveToPlayer()
    {
        float distanceFromTarget = Vector3.Distance(transform.position, player.transform.position);
        if (distanceFromTarget <= followRadius)
        {
            Vector3 targetPosition = player.transform.position;
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                2f * Time.deltaTime);
            transform.LookAt(player.transform);
        }
    }

    private void Update()
    {
        MoveToPlayer();
    }

    private void OnCollisionEnter(Collision collision)
    {
    }
}
