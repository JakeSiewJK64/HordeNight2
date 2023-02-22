using UnityEngine;

public class ViewholderItems
{
    public string name { get; set; }
    public string damage { get; set; }
    public string rateOfFire { get; set; }
    public string reloadSpeed { get; set; }
    public string magazineSize { get; set; }
    public string points { get; set; }
    public Sprite weaponProfile { get; set; }
    public Weapon weapon { get; set; }

    public ViewholderItems(
        string name, 
        string damage, 
        string rateOfFire, 
        string reloadSpeed, 
        string magazineSize, 
        string points, 
        Sprite weaponProfile,
        Weapon weapon)
    {
        this.weapon = weapon;
        this.name = name;
        this.damage = damage;
        this.rateOfFire = rateOfFire;
        this.reloadSpeed = reloadSpeed;
        this.magazineSize = magazineSize;
        this.points = points;
        this.weaponProfile = weaponProfile;
    }
}