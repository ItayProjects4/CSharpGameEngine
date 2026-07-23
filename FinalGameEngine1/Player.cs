using System;
using System.Collections.Generic;

namespace GameEngine
{
    public class Player : Entity
    {
        public int Health { get; set; }
        public int Score { get; set; }
        public int Damage { get; set; }
        public int AttackRange { get; set; } = 1;

        public Weapon EquippedWeapon { get; set; }
        public int Level { get; set; } = 1;
        public int XP { get; set; } = 0;
        public int XPToNextLevel { get; set; } = 100;

        private Random rand = new Random();
        public GameObjectManager Manager { get; set; }

        public bool IsDefending { get; set; } = false;
        public int Potions { get; set; } = 3;

        public Player(string name, int x, int y, int damage, GameObjectManager manager, Weapon equippedWeapon)
            : base(name, x, y)
        {
            Health = 100;
            Score = 0;
            Damage = damage;
            Manager = manager;
            EquippedWeapon = equippedWeapon;
            AttackRange = equippedWeapon.Range;
        }

        public void PerformCombatAction()
        {
            IsDefending = false;

            Console.WriteLine("\nactions:");
            Console.WriteLine("1 Attack Nearby Enemies");
            Console.WriteLine("2 Drink Potion +30 HP. " + Potions + " left");
            Console.WriteLine("3 Defend take Half damage this turn");
            Console.Write("Choice: ");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                AttackEnemies();
            }
            else if (choice == "2")
            {
                if (Potions > 0)
                {
                    Health += 30;
                    Potions--;
                    Manager.AddLog(Name + " drank a potion. Current HP: " + Health);
                }
                else
                {
                    Manager.AddLog("No potions left");
                }
            }
            else if (choice == "3")
            {
                IsDefending = true;
                Manager.AddLog(Name + " is defending");
            }
            else
            {
                Manager.AddLog("Invalid choice.. Turn skipped.");
            }
        }

        public void Move(ConsoleKey key)
        {
            if (key == ConsoleKey.W)
            {
                if (Y > 0) Y -= 1;
            }
            else if (key == ConsoleKey.S)
            {
                if (Y < 10) Y += 1;
            }
            else if (key == ConsoleKey.A)
            {
                if (X > 0) X -= 1;
            }
            else if (key == ConsoleKey.D)
            {
                if (X < 10) X += 1;
            }
        }

        public int CalculateDamage()
        {
            int chance = rand.Next(1, 101);
            if (chance <= 20)
            {
                Manager.AddLog("crit hit. well done. Double damage!");
                return EquippedWeapon.Damage * 2;
            }
            return EquippedWeapon.Damage;
        }

        public void AttackEnemies()
        {
            List<Enemy> nearbyEnemies = Manager.FindEnemiesNear(X, Y, AttackRange);

            if (nearbyEnemies.Count == 0)
            {
                Manager.AddLog("No enemies in range to attack");
                return;
            }

            for (int i = 0; i < nearbyEnemies.Count; i++)
            {
                Enemy enemy = nearbyEnemies[i];
                int actualDamage = CalculateDamage();
                enemy.TakeDamage(actualDamage);
                Manager.AddLog("Player " + Name + " attacked " + enemy.Name + " for " + actualDamage + " damage");

                if (enemy.Health <= 0)
                {
                    Score += 10;
                    GainXP(50);
                    Manager.AddLog("Player " + Name + " defeated " + enemy.Name + "! (+10 Score, +50 XP)");
                }
            }
        }

        public override void Update()
        {
            List<Enemy> nearby = Manager.FindEnemiesNear(X, Y, AttackRange);
            if (nearby.Count > 0)
            {
                PerformCombatAction();
            }
        }

        public override void Draw()
        {
            Console.WriteLine($"Player {Name}: Lvl {Level}. HP: {Health}. Score: {Score}. XP: {XP}/{XPToNextLevel}");
        }

        public void TakeDamage(int damage)
        {
            if (IsDefending)
            {
                damage /= 2;
                Manager.AddLog(Name + " blocked half damage");
            }

            Health -= damage;
            if (Health < 0) Health = 0;
        }

        public void GainXP(int amount)
        {
            XP += amount;
            Manager.AddLog(Name + " gained " + amount + " XP.");

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
            Manager.AddLog(Name + " leveled up! Now level " + Level + "!");
        }

        public void Explore()
        {
            int outcome = rand.Next(0, 4);
            if (outcome == 0)
            {
                int heal = rand.Next(10, 25);
                Health += heal;
                Manager.AddLog(Name + " found a small health thingi and recovered " + heal + " HP.");
            }
            else if (outcome == 1)
            {
                int xp = rand.Next(20, 40);
                GainXP(xp);
                Manager.AddLog(Name + " found an ancient scroll (+" + xp + " XP).");
            }
            else if (outcome == 2)
            {
                Potions++;
                Manager.AddLog(Name + " found a Potion. Total potions: " + Potions);
            }
            else if (outcome == 3)
            {
                Manager.AddLog(Name + " explored the area but found nothing.");
            }
        }

        public void EnchantWeapon()
        {
            int cost = EquippedWeapon.Level * 50;
            if (XP >= cost)
            {
                XP -= cost;
                EquippedWeapon.Upgrade();
                AttackRange = EquippedWeapon.Range;
                Manager.AddLog(EquippedWeapon.Name + " enchanted to Level " + EquippedWeapon.Level + "!");
            }
            else
            {
                Manager.AddLog("Not enough XP to enchant. Needs " + cost + " XP.");
            }
        }
    }
}