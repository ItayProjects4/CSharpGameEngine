using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameEngine
{
    public abstract class Entity : IEntity
    {
        public string Name { get; private set; }
        public int Y { get; protected set; }
        public int X { get; protected set; }

        public Entity(string name, int y, int x)
        {
            Name = name;
            Y = y;
            X = x;
        }
        public abstract void Update();
        public abstract void Draw();



    }
}
