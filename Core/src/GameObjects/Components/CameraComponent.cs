using MegamanX.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MegamanX.GameObjects.Components
{
    public class CameraComponent(GameWorld world, GameObject gameObject) : IComponent
    {
        public Vector2 Position => transformComponent.Position;

        public Rectangle Bounds { get; }

        public Rectangle WorldBounds => Bounds.Translate(transformComponent.Position);

        private readonly TransformComponent transformComponent = gameObject.GetComponent<TransformComponent>();

        void IComponent.Update(GameTime gameTime)
        {
            foreach (GameObject gameObject in world.Objects)
            {
                InteractiveComponent? interactive = gameObject.TryGetComponent<InteractiveComponent>();
                if (interactive == null || interactive.IsPersistent)
                {
                    continue;
                }
                gameObject.IsActive = WorldBounds.Intersects(interactive.WorldBounds);
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
