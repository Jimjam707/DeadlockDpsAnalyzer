using System;
using System.Collections.Generic;
using DeadlockDpsAnalyzer.Models;
using DeadlockDpsAnalyzer.Repositories;
using DeadlockDpsAnalyzer.Services;
using Microsoft.Extensions.Configuration.Json;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

class Program
{
    static void Main(string[] args)
    {
        // Build configuration to read appsettings.json
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        // Get connection string
        string connectionString = config.GetConnectionString("DeadlockDatabase");

        // Test connection
        using var connection = new MySqlConnection(connectionString);
        connection.Open();
        Console.WriteLine("Connected to MySQL successfully!");
        
        
        //Hero selection
        var heroes = HeroRepository.GetHeroes();

        Console.WriteLine("Available Heroes:");
        foreach (var hero in heroes)
            Console.WriteLine($"- {hero.Name}");

        Hero selectedHero = null;
        while (selectedHero == null)
        {
            Console.Write("Enter hero name: ");
            string input = Console.ReadLine()?.Trim();
            selectedHero = heroes.Find(h => string.Equals(h.Name, input, StringComparison.OrdinalIgnoreCase));
            if (selectedHero == null) Console.WriteLine("Hero not found. Try again.");
        }

        //Level input
        int level;
        while (true)
        {
            Console.Write($"Enter level for {selectedHero.Name} (1+): ");
            if (int.TryParse(Console.ReadLine(), out level) && level >= 1) break;
            Console.WriteLine("Invalid level, try again.");
        }
        selectedHero.Level = level;

        //Multiple item builds
        var allItems = ItemRepository.GetAllItems();
        List<(Hero hero, List<Item> items, double dps)> heroBuilds = new();
        bool tryAnotherBuild = true;

        while (tryAnotherBuild)
        {
            List<Item> selectedItems = new List<Item>();

            // Item selection
            Console.WriteLine("\nAvailable Items:");
            for (int i = 0; i < allItems.Count; i++)
            {
                var item = allItems[i];
                string damageDisplay = item.BoostedDamageType == DamageType.Flat
                                       ? item.DamageBoost.ToString()
                                       : (item.DamageBoost * 100) + "%";

                Console.WriteLine($"{i + 1}. {item.Name} (Damage: {damageDisplay}, Fire Rate: {item.FireRateBoost})");
            }

            Console.WriteLine("\nEnter item numbers separated by commas (e.g., 1,3), or leave blank for no items:");
            string itemInput = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(itemInput))
            {
                string[] parts = itemInput.Split(',');

                foreach (var part in parts)
                {
                    if (int.TryParse(part.Trim(), out int itemNum) && itemNum >= 1 && itemNum <= allItems.Count)
                    {
                        var itemToAdd = allItems[itemNum - 1];

                        if (!selectedItems.Contains(itemToAdd))
                            selectedItems.Add(itemToAdd);
                        else
                            Console.WriteLine($"Item '{itemToAdd.Name}' is already selected. Skipping duplicate.");
                    }
                }
            }

            //Show selected items
            if (selectedItems.Count > 0)
            {
                Console.WriteLine("\nYou selected the following items:");
                foreach (var item in selectedItems)
                {
                    string damageDisplay = item.BoostedDamageType == DamageType.Flat
                                           ? item.DamageBoost.ToString()
                                           : (item.DamageBoost * 100) + "%";

                    Console.WriteLine($"- {item.Name} (Damage: {damageDisplay}, Fire Rate: {item.FireRateBoost})");
                }
            }
            else
            {
                Console.WriteLine("\nNo items selected.");
            }

            //Confirmation prompt
            bool confirmed = false;
            while (!confirmed)
            {
                Console.Write("\nProceed to calculate DPS with these items? (Y/N): ");
                string input = Console.ReadLine()?.Trim().ToUpper();

                if (input == "Y")
                {
                    confirmed = true;
                }
                else if (input == "N")
                {
                    Console.WriteLine("You can reselect items now.");
                    selectedItems.Clear();
                    confirmed = true; 
                    continue; // restarts outer while loop
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter Y or N.");
                }
            }

            //DPS calculation
            var calculator = new DpsCalculator();
            double dps = calculator.CalculateDps(selectedHero, selectedItems);

            //Output
            Console.WriteLine($"\n{selectedHero.Name} at level {selectedHero.Level} with selected items:");
            Console.WriteLine($"Total DPS: {dps:F2}");

            //Store build for comparison
            heroBuilds.Add((selectedHero, new List<Item>(selectedItems), dps));

            //Ask if user wants to try another combination
            bool validResponse = false;
            while (!validResponse)
            {
                Console.Write("\nDo you want to try a different item combination for this hero? (Y/N): ");
                string input = Console.ReadLine()?.Trim().ToUpper();

                if (input == "Y")
                {
                    validResponse = true;
                    tryAnotherBuild = true;
                }
                else if (input == "N")
                {
                    validResponse = true;
                    tryAnotherBuild = false;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter Y or N.");
                }
            }
        }

        //Final summary of all builds
        Console.WriteLine("\nAll builds for this hero:");
        foreach (var build in heroBuilds)
        {
            string itemList = build.items.Count > 0 
                              ? string.Join(", ", build.items.ConvertAll(i => i.Name)) 
                              : "No items";

            Console.WriteLine($"- Items: {itemList} => DPS: {build.dps:F2}");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}


