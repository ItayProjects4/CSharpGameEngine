using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Player : Entity
    {
        public int Health { get; private set; }

        public int Score { get; private set; }

        public int Damage { get; private set; }

        public int AttackRange { get; set; } = 1;

        private int attackCooldown = 0;

        private int attackCooldownMax = 2;

        public Weapon EquippedWeapon { get; private set; }

        public int Level { get; private set; } = 1;

        public int XP { get; private set; } = 0;

        public int XPToNextLevel { get; private set; } = 100;

        private static Random rand = new Random();
        private GameObjectManager Manager { get; set; }

        private bool bossUnlocked = false;
        public Player(string name, int y, int x, int damage, GameObjectManager manager, Weapon equippedWeapon) : base(name, y, x)
        {
            Health = 100;
            Score = 0;
            Damage = damage;
            Manager = manager;
            EquippedWeapon = equippedWeapon;
        }

        public void Move(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.W:
                    if (Y > 0) Y -= 1;
                    break;
                case ConsoleKey.S:
                    if (Y < 10) Y += 1;
                    break;
                case ConsoleKey.A:
                    if (X > 0) X -= 1;
                    break;
                case ConsoleKey.D:
                    if (X < 10) X += 1;
                    break;
            }
        }


        public override void Update()
        {



            if (attackCooldown == 0)
            {
                List<Enemy> nearbyEnemies = Manager.FindEnemiesNear(X, Y, AttackRange);


                foreach (Enemy enemy in nearbyEnemies)
                {
                    enemy.TakeDamage(EquippedWeapon.Damage);
                    Console.WriteLine($"Player {Name} attacked {enemy.Name} with {EquippedWeapon.Name} for {EquippedWeapon.Damage} damage");

                    if (enemy.Health <= 0)
                    {
                        if (enemy.Health <= 0)
                        {
                            Score += 10;
                            GainXP(50);
                            Console.WriteLine($"Player {Name} defeated {enemy.Name} and gained 10 points and 50 XP");
                        }
                    }
                }
            }
            else
            {
                attackCooldown--;
            }
            CheckBossUnlock();

        }


        public override void Draw()
        {
            Console.WriteLine($"Player {Name} at {X},{Y} with HP {Health}, and damage {Damage}");
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0) Health = 0;
        }
        public void GainXP(int amount)
        {
            XP += amount;
            Console.WriteLine($"{Name} gained {amount} XP! Total XP: {XP}/{XPToNextLevel}");

            while (XP >= XPToNextLevel)
            {
                LevelUp();
            }
        }
        private void LevelUp()
        {
            XP -= XPToNextLevel;
            Level++;
            XPToNextLevel = (int)(XPToNextLevel * 1.5);

            Health += 20;


            Console.WriteLine($"{Name} leveled up! Now level {Level} with {Health} HP!");
        }


        public void Explore()
        {
            int outcome = rand.Next(0, 5);
            switch (outcome)
            {
                case 0:
                    int heal = rand.Next(10, 31);
                    Health += heal;
                    Console.WriteLine($"{Name} found a healing potion and got {heal} HP! Current HP: {Health}");
                    break;

                case 1:
                    int xp = rand.Next(20, 51);
                    GainXP(xp);
                    Console.WriteLine($"{Name} discovered xp potion and gained {xp} XP!");
                    break;

                case 2:
                    int score = rand.Next(5, 16);
                    Score += score;
                    Console.WriteLine($"{Name} found dead enemies and gained {score} score points Total Score: {Score}");
                    break;

                case 3:
                    Console.WriteLine($"{Name} encountered a monster! Prepare to fight!");

                    Enemy monster = Manager.entities.Find(e => e is Enemy) as Enemy;
                    if (monster != null)
                    {
                        Console.WriteLine($"A wild {monster.Name} appears with {monster.Health} HP!");
                    }
                    else
                    {
                        Console.WriteLine("there are no monsters nearby right now.");
                    }
                    break;

                case 4:
                    Console.WriteLine($"{Name} explores but finds nothing.");
                    break;
            }
        }
        public void EnchantWeapon()
        {
            int cost = EquippedWeapon.Level * 50;
            if (XP >= cost)
            {
                XP -= cost;
                EquippedWeapon.Enchant(cost);
            }
            else
            {
                Console.WriteLine($"Not enough XP to enchant {EquippedWeapon.Name}. Needs {cost} XP.");
            }
        }
        private void CheckBossUnlock()
        {
            if (Level >= 10 && !bossUnlocked)
            {
                bossUnlocked = true;
                Console.WriteLine("You are strong enough to face the Final Boss! Do you want to enter the Boss Room? (Y/N)");
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Y)
                {
                    EnterBossRoom();
                }
            }
        }

        private void EnterBossRoom()
        {
            Console.WriteLine("Entering the Boss Room...");

            Weapon bossWeapon = new Weapon("Diamond sword", 40, 1);
            Boss finalBoss = new Boss("Steve", 5, 5, bossWeapon, Manager, 10);
            Manager.AddEntity(finalBoss);

            X = 5;
            Y = 5;

        }
    }
}
