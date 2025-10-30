namespace DeadlockDpsAnalyzer.Models;

public class Weapon
{
    public string Name { get; set; }
    public double Damage { get; set; }
    public double BaseFireRate { get; set; }

    public Weapon(string name, double damage, double baseFireRate)
    {
        Name = name;
        Damage = damage;
        BaseFireRate = baseFireRate;
    }
}
