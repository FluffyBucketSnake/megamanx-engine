using Microsoft.Xna.Framework;

namespace MegamanX.Components
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
