using System;

namespace GameEngine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameObjectManager manager = new GameObjectManager();

            Weapon starterWeapon = new Weapon("Iron Sword", 15, 1);
            Player player = new Player("Hero", 5, 5, 10, manager, starterWeapon);
            manager.AddEntity(player);

            Weapon enemyWeapon = new Weapon("Club", 8, 1);
            manager.AddEntity(new Enemy("Goblin", 2, 2, 5, manager, enemyWeapon));
            manager.AddEntity(new Enemy("Orc", 8, 8, 8, manager, enemyWeapon));

            bool running = true;
            while (running)
            {
                Console.Clear();

                manager.SpawnEnemiesIfNeeded(1);

                manager.DrawAll();
                manager.TurnLogs.Clear();

                Console.WriteLine("\nControls: W/A/S/D = Move. E = Explore. Q = Enchant. X = Quit");
                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.W || key == ConsoleKey.A || key == ConsoleKey.S || key == ConsoleKey.D)
                {
                    player.Move(key);
                }
                else if (key == ConsoleKey.E)
                {
                    player.Explore();
                }
                else if (key == ConsoleKey.Q)
                {
                    player.EnchantWeapon();
                }
                else if (key == ConsoleKey.X)
                {
                    running = false;
                    Console.WriteLine("Goodbye!");
                    continue;
                }
                else
                {
                    manager.AddLog("Invalid key command!");
                    continue;
                }

                manager.UpdateAll();
            }
        }
    }
}