using DeadlockDpsAnalyzer.Models;
namespace DeadlockDpsAnalyzer.Repositories;

public class ItemRepository
{
    public static List<Item> GetAllItems()
    {
        return new List<Item>
        {
            new Item("Headshot Booster", damageBoost: 50, fireRateBoost: 0.2, damageType: DamageType.Flat ),
            new Item("Backstabber", damageBoost: 10, fireRateBoost: 0, damageType: DamageType.Percent),
            new Item("Mystic Shot", damageBoost: 55, fireRateBoost: 0, damageType: DamageType.Flat)
            // Add more items here
        };
    }    
}