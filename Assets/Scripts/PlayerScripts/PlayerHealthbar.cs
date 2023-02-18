using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthbar : MonoBehaviour
{
    public Player player;
    public float lastDamageTime;

    [SerializeField]
    private Image healthbar;
    private float healthbarLength = .8f;
    private float healthbarHeight = .0625f;
    private float healthRegenBuffer = 5f;

    void Start()
    {
        player = new Player(100, 100, 5, 3f);
    }

    public void TakeDamage(float damage)
    {
        player.TakeDamage(damage);
        lastDamageTime = Time.time;
    }


    private void Update()
    {
        if (player.health < 100)
        {
            // player dies
            if (player.health <= 0)
            {
                ProcessDeath();
            }

            if (Time.time - lastDamageTime > healthRegenBuffer)
            {
                // regenerate health
                GetComponent<PlayerHealthbar>().player.GainHealth(Time.deltaTime * player.healthRegeneration);
            }
        }

        healthbar.GetComponent<RectTransform>().sizeDelta = new Vector2((player.health / 100) * healthbarLength, healthbarHeight);
    }

    private void ProcessDeath()
    {
        gameObject.GetComponent<PlayerDeath>().PromptDeathScreen();
    }
}
