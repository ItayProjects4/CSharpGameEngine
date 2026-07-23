using System;

namespace GameEngine
{
    public abstract class Entity : IEntity
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Entity(string name, int x, int y)
        {
            Name = name;
            X = x;
            Y = y;
        }

        public abstract void Update();
        public abstract void Draw();
    }
}