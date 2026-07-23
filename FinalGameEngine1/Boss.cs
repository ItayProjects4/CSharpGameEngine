
using System;

namespace GameEngine
{
    public class Boss : Enemy
    {
        public Boss(string name, int x, int y, Weapon weapon, GameObjectManager manager, int level = 10)
            : base(name, x, y, 50, manager, weapon, level)
        {
        }

        public override void Update()
        {
            base.Update();
        }

        public void SpecialAttack(Player player)
        {
            int damage = EquippedWeapon.Damage + 20;
            player.TakeDamage(damage);
            Console.WriteLine(Name + " used a special attack on " + player.Name + " for " + damage + " damage!");
        }
    }
}