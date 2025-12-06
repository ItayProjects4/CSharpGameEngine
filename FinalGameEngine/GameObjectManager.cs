using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class GameObjectManager
    {

        public List<Entity> entities = new List<Entity>();

        public void AddEntity(Entity entity)
        {
            bool exists = false;
            foreach (var e in entities)
            {
                if (e.Name == entity.Name)
                {
                    exists = true;
                }
            }

            if (!exists)
            {
                entities.Add(entity);
            }
            else
            {
                throw new EntityAlreadyExistsException("Entity " + entity.Name + " already exists!");
            }
        }

        public void RemoveEntity(Entity entity)
        {
            if (!entities.Contains(entity))
            {
                entities.Remove(entity);
            }
            else
            {
                throw new EntityDoesntExistException("entity " + entity.Name + " does not exist");
            }

        }
        public void UpdateAll()
        {
            List<Entity> copy = new List<Entity>(entities);

            foreach (var entity in copy)
            {
                entity.Update();

                if (entity is Player)
                {
                    Player p = (Player)entity;
                    if (p.Health <= 0)
                    {
                        Console.WriteLine("Player " + p.Name + " died!");
                        RemoveEntity(p);
                    }
                }
                else if (entity is Enemy)
                {
                    Enemy e = (Enemy)entity;
                    if (e.Health <= 0)
                    {
                        Console.WriteLine("Enemy " + e.Name + " defeated!");
                        RemoveEntity(e);
                    }
                }
            }
        }
        public void DrawAll()
        {
            foreach (var entity in entities)
            {
                entity.Draw();
            }
        }
        public Entity GetEntityByName(string name)
        {
            foreach (var e in entities)
            {
                if (e.Name == name)
                {
                    return e;
                }
            }
            return null;
        }

        public List<Enemy> FindEnemiesNear(int x, int y, int range)
        {
            List<Enemy> result = new List<Enemy>();

            foreach (var entity in entities)
            {
                if (entity is Enemy)
                {
                    Enemy e = (Enemy)entity;
                    int dx = e.X - x;
                    int dy = e.Y - y;
                    if (dx < 0) dx = -dx;
                    if (dy < 0) dy = -dy;

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
