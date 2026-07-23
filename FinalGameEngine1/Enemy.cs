
using System;
using System.Collections.Generic;

namespace GameEngine
{
    public class Enemy : Entity
    {
        public int Health { get; set; }
        public int Damage { get; set; }
        public int Level { get; set; } = 1;
        public int XP { get; set; } = 0;
        public int XPToNextLevel { get; set; } = 50;
        public Weapon EquippedWeapon { get; set; }
        public GameObjectManager Manager { get; set; }

        private Random rand = new Random();
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
            Player targetPlayer = FindClosestPlayer();

            if (targetPlayer != null)
            {
                int dist = Math.Abs(targetPlayer.X - X) + Math.Abs(targetPlayer.Y - Y);

                if (dist <= 4 && dist > EquippedWeapon.Range)
                {
                    int stepX = 0;
                    int stepY = 0;

                    if (targetPlayer.X > X) stepX = 1;
                    else if (targetPlayer.X < X) stepX = -1;

                    if (targetPlayer.Y > Y) stepY = 1;
                    else if (targetPlayer.Y < Y) stepY = -1;

                    Move(stepX, stepY);
                }
                else if (dist > 4)
                {
                    int dx = rand.Next(-1, 2);
                    int dy = rand.Next(-1, 2);
                    Move(dx, dy);
                }
            }

            if (attackCooldown == 0)
            {
                Player target = FindClosestPlayerInRange(EquippedWeapon.Range);
                if (target != null)
                {
                    target.TakeDamage(EquippedWeapon.Damage);
                    Manager.AddLog("Enemy " + Name + " attacked " + target.Name + " for " + EquippedWeapon.Damage + " damage");
                    attackCooldown = attackCooldownMax;
                }
            }
            else
            {
                attackCooldown--;
            }
        }

        public override void Draw()
        {
            Console.WriteLine($"Enemy {Name}: Lvl {Level} at ({X},{Y}). HP: {Health}. Wpn: {EquippedWeapon.Name}");
        }

        public void TakeDamage(int damage)
        {
            if (rand.Next(1, 101) <= 15)
            {
                Manager.AddLog(Name + " dodged the attack");
                return;
            }

            Health -= damage;
            if (Health < 0) Health = 0;
            Manager.AddLog(Name + " took " + damage + " damage. HP: " + Health);
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

        private Player FindClosestPlayer()
        {
            Player closest = null;
            int minDist = 9999;

            for (int i = 0; i < Manager.entities.Count; i++)
            {
                if (Manager.entities[i] is Player)
                {
                    Player p = (Player)Manager.entities[i];
                    int dist = Math.Abs(p.X - X) + Math.Abs(p.Y - Y);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        closest = p;
                    }
                }
            }
            return closest;
        }

        private Player FindClosestPlayerInRange(int range)
        {
            Player closest = null;
            int minDist = 9999;

            for (int i = 0; i < Manager.entities.Count; i++)
            {
                if (Manager.entities[i] is Player)
                {
                    Player p = (Player)Manager.entities[i];
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
    }
}