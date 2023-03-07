using System.Collections.Generic;

public class Player : Character
{
    public float stamina { get; set; }
    public bool immune { get; set; }
    public float healthRegeneration { get; set; }
    public float playerSpeed { get; set; }

    public Dictionary<string, int> upgradeModule { get; set; }

    public Player(float health, float stamina, float damage, float healthRegeneration) : base(health, damage)
    {
        this.stamina = stamina;
        this.healthRegeneration = healthRegeneration;
        playerSpeed = 5f;
        upgradeModule = new Dictionary<string, int>()
        {
            { "MOVEMENT_SPEED", 0 },
            { "HEALTH", 0 },
            { "HEALTH_REGEN_SPEED", 0 },
        };
    }

    public void LevelUpModule(string tag)
    {
        upgradeModule[tag]++;
    } 

    public float GetHealth()
    {
        return health + health * (upgradeModule["HEALTH"] * .25f);
    }

    public float GetSpeed()
    {
        return playerSpeed + playerSpeed * (upgradeModule["MOVEMENT_SPEED"] * .25f);
    }

    public float GetHealthRegenSpeed()
    {
        return healthRegeneration + healthRegeneration * (upgradeModule["HEALTH_REGEN_SPEED"] * .25f);
    }

    public void TakeDamage(float damage)
    {
        if (!immune)
        {
            health -= damage;
        }
    }

    public void GainHealth(float amount)
    {
        health += amount;
    }
}