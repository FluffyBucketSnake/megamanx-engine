using Microsoft.Xna.Framework;

namespace MegamanX.Components
{
    public class InteractiveComponent(Entity entity, Rectangle bounds, bool isActive = false, bool isPersistent = false) : IComponent
    {
        public bool IsPersistent => isPersistent;

        public bool IsActive => isActive;

        public Rectangle Bounds { get => bounds; set => bounds = value; }

        public Rectangle WorldBounds => Bounds.Translate(transform.Position);

        private readonly TransformComponent transform = entity.GetComponent<TransformComponent>();

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
