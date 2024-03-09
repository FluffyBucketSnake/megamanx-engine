using Microsoft.Xna.Framework;

namespace MegamanX.GameObjects.Components
{
    public class InteractiveComponent(GameObject parent, Rectangle bounds, bool isActive = false, bool isPersistent = false) : IComponent
    {
        public bool IsPersistent => isPersistent;

        public bool IsActive => isActive;

        public Rectangle Bounds { get => bounds; set => bounds = value; }

        public Rectangle WorldBounds => Bounds.Translate(transform.Position);

        private readonly TransformComponent transform = parent.GetComponent<TransformComponent>();

        public void Activate()
        {
            isActive = true;
        }

        public void Deactivate()
        {
            isActive = false;
        }
    }
}
