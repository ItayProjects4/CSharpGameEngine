using System;

namespace GameEngine
{
    public class Weapon
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public int Range { get; set; }
        public int Level { get; set; } = 1;

        public Weapon(string name, int damage, int range)
        {
            Name = name;
            Damage = damage;
            Range = range;
        }

        public void Upgrade()
        {
            Damage += 5;
            Range += 1;
            Level++;
        }
    }
}