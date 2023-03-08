using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthbar : MonoBehaviour
{
    public Player player;
    public float lastDamageTime;

    [SerializeField]
    private Image healthbar;

    [SerializeField]
    private TextMeshProUGUI healthIndicator;

    private float healthbarLength = .8f;
    private float healthbarHeight = .0625f;

    void Start()
    {
        player = new Player(
            health: 100, 
            stamina: 100, 
            damage: 5, 
            healthRegeneration: 3f
        );
    }

    public void TakeDamage(float damage)
    {
        player.TakeDamage(damage);
        lastDamageTime = Time.time;

        if (player.health <= 0)
        {
            ProcessDeath();
        }
    }

    private void Update()
    {
        if (Time.time - lastDamageTime > player.GetHealthRegenDelay() && player.health < player.GetMaxHealth())
        {
            // regenerate health
            GetComponent<PlayerHealthbar>().player.GainHealth(Time.deltaTime * player.GetHealthRegenSpeed());
        }
        healthIndicator.text = Math.Round(player.health, 0) + " / " + Math.Round(player.GetMaxHealth(), 0);
        healthbar.GetComponent<RectTransform>().sizeDelta = new Vector2(player.health / player.GetMaxHealth() * healthbarLength, healthbarHeight);
    }

    private void ProcessDeath()
    {
        gameObject.GetComponent<PlayerDeath>().PromptDeathScreen();
    }
}
