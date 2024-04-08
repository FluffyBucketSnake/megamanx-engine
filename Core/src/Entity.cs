using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MegamanX
{
    public class Entity
    {
        public string? Name { get; set; }
        public bool IsActive { get; set; } = true;

        private readonly List<IComponent> updatableComponents = [];
        private readonly List<IComponent> postUpdatableComponents = [];
        private readonly List<IComponent> drawableComponents = [];
        private readonly List<IComponent> components = [];

        public void AddComponent(IComponent component)
        {
            // TODO: test & bench binary search insert
            if (component.DrawPriority != null)
            {
                drawableComponents.Add(component);
                drawableComponents.Sort(Comparer<IComponent>.Create((a, b) => a.DrawPriority!.Value.CompareTo(b.DrawPriority!.Value)));
            }
            if (component.UpdatePriority != null)
            {
                updatableComponents.Add(component);
                updatableComponents.Sort(Comparer<IComponent>.Create((a, b) => a.UpdatePriority!.Value.CompareTo(b.UpdatePriority!.Value)));

            }
            if (component.PostUpdatePriority != null)
            {
                postUpdatableComponents.Add(component);
                postUpdatableComponents.Sort(Comparer<IComponent>.Create((a, b) => a.UpdatePriority!.Value.CompareTo(b.UpdatePriority!.Value)));

            }
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

        // TODO: Test & benchmark sending these methods into GameWorld and order components by priority then type
        public void Update(GameTime gameTime)
        {
            foreach (IComponent component in updatableComponents)
            {
                component.Update(gameTime);
            }
        }

        public void PostUpdate(GameTime gameTime)
        {
            foreach (IComponent component in postUpdatableComponents)
            {
                component.PostUpdate(gameTime);
            }

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (IComponent component in drawableComponents)
            {
                component.Draw(gameTime, spriteBatch);
            }
        }
    }

}
