using System;
using System.Collections.Generic;

namespace GameEngine
{
    public class GameObjectManager
    {
        public List<Entity> entities = new List<Entity>();
        public List<string> TurnLogs = new List<string>();
        private Random rand = new Random();

        public void AddLog(string message)
        {
            TurnLogs.Add(message);
        }

        public void SpawnEnemiesIfNeeded(int minEnemies = 2)
        {
            int currentEnemyCount = 0;
            Player player = null;

            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i] is Enemy) currentEnemyCount++;
                if (entities[i] is Player) player = (Player)entities[i];
            }

            while (currentEnemyCount < minEnemies)
            {
                int spawnX = rand.Next(0, 11);
                int spawnY = rand.Next(0, 11);

                if (player != null && Math.Abs(spawnX - player.X) <= 1 && Math.Abs(spawnY - player.Y) <= 1)
                {
                    continue;
                }

                string[] names = new string[] { "Goblin", "Orc", "Skeleton", "Bandit" };
                string enemyName = names[rand.Next(names.Length)];

                Weapon enemyWeapon = new Weapon("Rusty Blade", 6, 1);
                int level = (player != null) ? player.Level : 1;

                Enemy newEnemy = new Enemy(enemyName, spawnX, spawnY, 5, this, enemyWeapon, level);
                AddEntity(newEnemy);

                AddLog("A level " + level + " " + enemyName + " has spawned at (" + spawnX + "," + spawnY + ")!");
                currentEnemyCount++;
            }
        }

        public void AddEntity(Entity entity)
        {
            bool exists = false;
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].Name == entity.Name)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                entities.Add(entity);
            }
            else
            {
                Console.WriteLine("Warning: Entity " + entity.Name + " already exists!");
            }
        }

        public void RemoveEntity(Entity entity)
        {
            if (entities.Contains(entity))
            {
                entities.Remove(entity);
            }
            else
            {
                Console.WriteLine("Warning: Entity " + entity.Name + " does not exist!");
            }
        }

        public void UpdateAll()
        {
            for (int i = entities.Count - 1; i >= 0; i--)
            {
                Entity entity = entities[i];
                entity.Update();

                if (entity is Player)
                {
                    Player p = (Player)entity;
                    if (p.Health <= 0)
                    {
                        AddLog("Player " + p.Name + " died");
                        RemoveEntity(p);
                    }
                }
                else if (entity is Enemy)
                {
                    Enemy e = (Enemy)entity;
                    if (e.Health <= 0)
                    {
                        AddLog("Enemy " + e.Name + " defeated");
                        RemoveEntity(e);
                    }
                }
            }
        }

        public void DrawAll()
        {
            Console.WriteLine();
            for (int y = 0; y <= 10; y++)
            {
                for (int x = 0; x <= 10; x++)
                {
                    string symbol = ". ";
                    for (int i = 0; i < entities.Count; i++)
                    {
                        if (entities[i].X == x && entities[i].Y == y)
                        {
                            if (entities[i] is Player) symbol = "P ";
                            else if (entities[i] is Enemy) symbol = "E ";
                        }
                    }
                    Console.Write(symbol);
                }
                Console.WriteLine();
            }

            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Draw();
            }

            if (TurnLogs.Count > 0)
            {
                Console.WriteLine("\nTURN EVENTS");
                for (int i = 0; i < TurnLogs.Count; i++)
                {
                    Console.WriteLine("> " + TurnLogs[i]);
                }
            }
        }

        public List<Enemy> FindEnemiesNear(int x, int y, int range)
        {
            List<Enemy> result = new List<Enemy>();

            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i] is Enemy)
                {
                    Enemy e = (Enemy)entities[i];
                    int dx = Math.Abs(e.X - x);
                    int dy = Math.Abs(e.Y - y);

                    if (dx <= range && dy <= range)
                    {
                        result.Add(e);
                    }
                }
            }

            return result;
        }
    }
}