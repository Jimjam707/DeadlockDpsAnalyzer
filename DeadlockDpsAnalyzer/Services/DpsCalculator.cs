using DeadlockDpsAnalyzer.Models;
namespace DeadlockDpsAnalyzer.Services;

public class DpsCalculator
{
    public double CalculateDps(Hero hero, List<Item> items)
    {
        
        double totalDamage = hero.GetScaledDamage();
        double totalAttackSpeed = hero.GetScaledFireRate();

        
        foreach (var item in items)
        {
            
            if (item.BoostedDamageType == DamageType.Flat)
            {
                totalDamage += item.DamageBoost;
            }
            else if (item.BoostedDamageType == DamageType.Percent)
            {
                totalDamage += totalDamage * item.DamageBoost; 
            }

            
            totalAttackSpeed += item.FireRateBoost;
        }

        
        return totalDamage * totalAttackSpeed;
    }
}

