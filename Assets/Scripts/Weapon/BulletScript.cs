using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Dictionary<string, System.Action<Collision>> actions 
        = new Dictionary<string, System.Action<Collision>>();

    void Start()
    {
        actions.Add("Environment", HandleEnvironment);
        actions.Add("Zombie", HandleZombie);
    }

    private void HandleEnvironment(Collision obj)
    {
        Destroy(gameObject);
    }

    private void HandleZombie(Collision obj)
    {
        Destroy(obj.gameObject);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;
        if (actions.ContainsKey(tag))
        {
            actions[tag](collision);
        }
    }
}
