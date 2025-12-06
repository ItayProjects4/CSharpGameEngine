using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Weapon
{
    public string Name { get; private set; }
    public int Damage { get; private set; }
    public int Range { get; private set; }
    public int Level { get; private set; } = 1;

    public Weapon(string name, int damage, int range)
    {
        Name = name;
        Damage = damage;
        Range = range;
    }

    public void Enchant(int playerXP)
    {
        int cost = Level * 50;
        if (playerXP >= cost)
        {
            Damage += 5;
            Range += 1;
            Level++;
            Console.WriteLine($"{Name} enchanted! Now Level {Level}, Damage {Damage}, Range {Range}");
        }
        else
        {
            Console.WriteLine($"Not enough XP to enchant {Name}. Needs {cost} XP.");
        }
    }
}
