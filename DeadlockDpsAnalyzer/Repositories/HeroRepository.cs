using System.Collections.Generic;
using DeadlockDpsAnalyzer.Models;

namespace DeadlockDpsAnalyzer.Repositories;

public class HeroRepository
{
    public static List<Hero> GetHeroes()
    {
    var vindictaWeapon = new Weapon("Vindicta", 50, 1.2);
    var greyTalonWeapon = new Weapon("Grey Talon", 45, 1.5);
    var wardenWeapon = new Weapon("Warden", 60, 1.0);

    var heroes = new List<Hero>
        {
            new Hero("Vindicta", vindictaWeapon, damagePerLevel: 5, fireRatePerLevel: 0.05),
            new Hero("Grey Talon", greyTalonWeapon, damagePerLevel: 4, fireRatePerLevel: 0.07),
            new Hero("Warden", wardenWeapon, damagePerLevel: 6, fireRatePerLevel: 0.03)
        };

        return heroes;
    }
}