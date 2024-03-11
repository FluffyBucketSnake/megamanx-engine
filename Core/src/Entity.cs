using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MegamanX
{
    public class Entity
    {
        public string? Name { get; set; }

        public bool IsActive { get; set; } = true;

        private readonly List<IComponent> components = [];

        public void AddComponent(IComponent component)
        {
            components.Add(component);
        }

        public IComponent? TryGetComponent(System.Type componentType)
        {
            return components.Find(component => component.GetType() == componentType);
        }

        public T? TryGetComponent<T>() where T : IComponent
        {
            return (T?)TryGetComponent(typeof(T));
        }

        public IComponent GetComponent(System.Type componentType)
        {
            return TryGetComponent(componentType) ?? throw new KeyNotFoundException("Couldn't find the specified component type.");
        }

        public T GetComponent<T>() where T : IComponent
        {
            return (T)GetComponent(typeof(T));
        }

        public void Update(GameTime gameTime)
        {
            foreach (IComponent component in components)
            {
                component.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (IComponent component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }
        }
    }

}
