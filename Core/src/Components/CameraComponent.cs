using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MegamanX.Components
{
    public class CameraComponent(GameWorld world, Entity entity) : IComponent
    {
        public Vector2 Position => transformComponent.Position;

        public Rectangle Bounds { get; }

        public Rectangle WorldBounds => Bounds.Translate(transformComponent.Position);

        private readonly TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

        void IComponent.Update(GameTime gameTime)
        {
            foreach (Entity entity in world.Entities)
            {
                InteractiveComponent? interactive = entity.TryGetComponent<InteractiveComponent>();
                if (interactive == null || interactive.IsPersistent)
                {
                    continue;
                }
                entity.IsActive = WorldBounds.Intersects(interactive.WorldBounds);
            }
        }

        public Matrix GetTransformation(Viewport targetViewport)
        {
            Vector2 translation;
            translation.X = -((int)Position.X + Bounds.X);
            translation.Y = -((int)Position.Y + Bounds.Y);

            Vector2 scale;
            scale.X = targetViewport.Width / Bounds.Width;
            scale.Y = targetViewport.Height / Bounds.Height;

            return Matrix.CreateTranslation(translation.X, translation.Y, 0) *
                Matrix.CreateScale(scale.X, scale.Y, 1);
        }
    }
}
