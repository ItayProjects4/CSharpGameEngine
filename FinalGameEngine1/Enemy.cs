using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;


namespace GameEngine
{
    public class Enemy : Entity
    {
        public int Health { get; private set; }
        public int Damage { get; private set; }
        public int Level { get; private set; } = 1;
        public int XP { get; private set; } = 0;
        public int XPToNextLevel { get; private set; } = 50;
        public Weapon EquippedWeapon { get; private set; }
        private GameObjectManager Manager { get; set; }

        private static Random rand = new Random();

        private int attackCooldown = 0;

        private int attackCooldownMax = 2;

        public Enemy(string name, int x, int y, int damage, GameObjectManager manager, Weapon equippedWeapon, int level = 1)
            : base(name, x, y)
        {
            Level = level;
            Health = 100 * Level;
            Damage = damage * Level;
            Manager = manager;
            EquippedWeapon = equippedWeapon;
        }

        public override void Update()
        {

            int dx = rand.Next(-1, 2);
            int dy = rand.Next(-1, 2);
            Move(dx, dy);


            GainXP(1);


            if (attackCooldown == 0)
            {
                Player target = FindClosestPlayerInRange(EquippedWeapon.Range);
                if (target != null)
                {
                    target.TakeDamage(EquippedWeapon.Damage);
                    Console.WriteLine($"Enemy {Name} attacked {target.Name} with {EquippedWeapon.Name} for {EquippedWeapon.Damage} damage");
                }
                attackCooldown = attackCooldownMax;
            }
            else
            {
                attackCooldown--;
            }
        }

        public override void Draw()
        {
            Console.WriteLine($"Enemy {Name} (Level {Level}) at ({X},{Y}) with HP {Health}, Damage {Damage}, Weapon: {EquippedWeapon.Name}");
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0)
                throw new Exception("Damage cannot be negative or zero");

            Health -= damage;
            if (Health < 0) Health = 0;
            if (Health > 0) Console.WriteLine($"{Name} has {Health} HP left");
        }

        private void Move(int deltaX, int deltaY)
        {
            X += deltaX;
            Y += deltaY;

            if (X < 0) X = 0;
            if (X > 10) X = 10;
            if (Y < 0) Y = 0;
            if (Y > 10) Y = 10;
        }

        private List<Player> FindPlayersNear(int range)
        {
            List<Player> result = new List<Player>();
            foreach (var entity in Manager.entities)
            {
                if (entity is Player p)
                {
                    int dx = Math.Abs(p.X - X);
                    int dy = Math.Abs(p.Y - Y);
                    if (dx <= range && dy <= range)
                    {
                        result.Add(p);
                    }
                }
            }
            return result;
        }

        private Player FindClosestPlayerInRange(int range)
        {
            Player closest = null;
            int minDist = int.MaxValue;
            foreach (var entity in Manager.entities)
            {
                if (entity is Player p)
                {
                    int dist = Math.Abs(p.X - X) + Math.Abs(p.Y - Y);
                    if (dist <= range && dist < minDist)
                    {
                        minDist = dist;
                        closest = p;
                    }
                }
            }
            return closest;
        }

        private void GainXP(int amount)
        {
            XP += amount;
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
            Health = 100 * Level;
            Damage += 5;
            Console.WriteLine($"{Name} leveled up! Now Level {Level}, HP {Health}, Damage {Damage}");
        }
    }
}
