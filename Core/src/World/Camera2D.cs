using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MegamanX
{
    public struct Camera2D
    {
        public static Camera2D Default => new Camera2D(new Rectangle(-128, -112, 256, 224));

        public Vector2 WorldPosition;

        public Rectangle SourceBounds;

        public Rectangle WorldBounds => SourceBounds.Translate(WorldPosition);

        public Camera2D(Rectangle sourceBounds)
        {
            SourceBounds = sourceBounds;
            WorldPosition = Vector2.Zero;
        }

        public Matrix GetTransformation(Viewport targetViewport)
        {
            Vector2 translation;
            translation.X = -((int)WorldPosition.X + SourceBounds.X);
            translation.Y = -((int)WorldPosition.Y + SourceBounds.Y);

            Vector2 scale;
            scale.X = targetViewport.Width / SourceBounds.Width;
            scale.Y = targetViewport.Height / SourceBounds.Height;

            return Matrix.CreateTranslation(translation.X, translation.Y, 0) * 
                Matrix.CreateScale(scale.X, scale.Y, 1);
        }
    }
}