using System.Collections.Generic;

public class Player : Character
{
    public float stamina { get; set; }
    public bool immune { get; set; }
    public float healthRegeneration { get; set; }
    public float playerSpeed { get; set; }

    public float maxHealth { get; set; }

    public Dictionary<string, int> upgradeModule { get; set; }

    public Player(float health, float stamina, float damage, float healthRegeneration) : base(health, damage)
    {
        this.stamina = stamina;
        this.healthRegeneration = healthRegeneration;
        playerSpeed = 5f;
        maxHealth = health;
        
        upgradeModule = new Dictionary<string, int>()
        {
            { "MovementSpeed", 0 },
            { "MaximumHealth", 0 },
            { "HealthRegenDelay", 0 },
            { "HealthRegenSpeed", 0 },
        };
    }

    public bool CanUpgrade(string tag)
    {
        return upgradeModule.ContainsKey(tag) && upgradeModule[tag] < 5f;
    }

    public void LevelUpModule(string tag)
    {
        if(tag == "MaximumHealth")
        {
            UpgradeMaxHealth();
        }

        upgradeModule[tag]++;
    } 

    public void UpgradeMaxHealth()
    {
        maxHealth = GetMaxHealth();
    }

    public float GetMaxHealth()
    {
        return maxHealth + maxHealth * (upgradeModule["MaximumHealth"] * .05f);
    }

    public float GetSpeed()
    {
        return playerSpeed + playerSpeed * (upgradeModule["MovementSpeed"] * .1f);
    }

    public float GetHealthRegenDelay()
    {
        return healthRegeneration - healthRegeneration * (upgradeModule["HealthRegenDelay"] * .15f);
    }

    public float GetHealthRegenSpeed()
    {
        return healthRegeneration + healthRegeneration * (upgradeModule["HealthRegenSpeed"] * .25f);
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