namespace DeadlockDpsAnalyzer.Models;

public enum DamageType
{
    Flat,
    Percent
}

public class Item
{
    public string Name { get; set; }
    public double DamageBoost { get; set; }
    public DamageType BoostedDamageType { get; set; }   
    public double FireRateBoost { get; set; }

    public Item(string name, double damageBoost, DamageType damageType, double fireRateBoost)
    {
        Name = name;
        DamageBoost = damageBoost;
        BoostedDamageType = damageType;
        FireRateBoost = fireRateBoost;
    }
}