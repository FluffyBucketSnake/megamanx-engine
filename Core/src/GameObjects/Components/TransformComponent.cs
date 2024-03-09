using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MegamanX.GameObjects.Components
{
    public class TransformComponent(Vector2 position) : IComponent
    {
        public Vector2 Position
        {
            get => position;
            set => position = value;
        }
    }
}
