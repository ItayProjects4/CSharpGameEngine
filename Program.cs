using System;
using System.Linq;
using System.Collections.Generic;
using GameEngine;

namespace GameEngine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameObjectManager manager = new GameObjectManager();

            // Create player
            Weapon starterWeapon = new Weapon("Iron Sword", 10, 1);
            Player player = new Player("Hero", 5, 5, 10, manager, starterWeapon);
            manager.AddEntity(player);

            // Create enemies
            Weapon enemyWeapon = new Weapon("Club", 5, 1);
            manager.AddEntity(new Enemy("Goblin", 2, 2, 5, manager, enemyWeapon));
            manager.AddEntity(new Enemy("Orc", 8, 8, 8, manager, enemyWeapon));

            bool running = true;

            while (running)
            {

                Console.Clear();
                manager.DrawAll();
                Console.WriteLine();
                Console.WriteLine("Commands: W/A/S/D to move, E to explore, Q to enchant weapon, X to quit");


                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.A:
                    case ConsoleKey.S:
                    case ConsoleKey.D:
                        player.Move(key);
                        break;
                    case ConsoleKey.E:
                        player.Explore();
                        break;
                    case ConsoleKey.Q:
                        player.EnchantWeapon();
                        break;
                    case ConsoleKey.X:
                        running = false;
                        Console.WriteLine("byebye");
                        continue;
                    default:
                        Console.WriteLine("Invalid command!");
                        continue;
                }


                player.Update();


                foreach (var enemy in manager.entities.OfType<Enemy>().ToList())
                {
                    enemy.Update();
                }


                manager.UpdateAll();


                Console.WriteLine("\nPress any key for next turn");
                Console.ReadKey(true);
            }
        }
    }
}
