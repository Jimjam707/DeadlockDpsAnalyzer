namespace DeadlockDpsAnalyzer.Models
{
    public class Hero
    {
        public string Name { get; set; }
        public int Level { get; set; }           
        public Weapon HeroWeapon { get; set; }
        
        public double DamagePerLevel { get; set; }     
        public double FireRatePerLevel { get; set; }   

        public Hero(string name, Weapon weapon, double damagePerLevel, double fireRatePerLevel)
        {
            Name = name;
            Level = 1;                                
            HeroWeapon = weapon;
            DamagePerLevel = damagePerLevel;
            FireRatePerLevel = fireRatePerLevel;
        }

        
        public double GetScaledDamage()
        {
            return HeroWeapon.Damage + (Level - 1) * DamagePerLevel;
        }

        public double GetScaledFireRate()
        {
            return HeroWeapon.BaseFireRate + (Level - 1) * FireRatePerLevel;
        }
    }
}