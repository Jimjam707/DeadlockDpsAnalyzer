using DeadlockDpsAnalyzer.Models;
using MySql.Data.MySqlClient;

namespace DeadlockDpsAnalyzer.Repositories;

public class HeroRepository
{
    private readonly string _connectionString;

    public HeroRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Hero> GetHeroes()
    {
        var heroes = new List<Hero>();

        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        string query = "SELECT Name, BaseDamage, BaseFireRate, DamagePerLevel FROM heroes;";
        using var cmd = new MySqlCommand(query, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            var weapon = new Weapon(
                reader.GetString("Name"),         // weapon name same as hero
                reader.GetDouble("BaseDamage"),   // weapon base damage
                reader.GetDouble("BaseFireRate")  // weapon base fire rate
            );

            heroes.Add(new Hero(
                reader.GetString("Name"),
                weapon,
                damagePerLevel: reader.GetDouble("DamagePerLevel"),
                fireRatePerLevel: 0 // optional: can remove if unused
            ));
        }

        return heroes;
    }
}
