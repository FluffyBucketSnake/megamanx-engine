using System.Text;
using MegamanX.GameObjects.Playable;

namespace MegamanX.GameObjects.Debug
{
    public class EntityDebugProfiler : IDebugProfiler
    {
        public void BuildText(GameObject target, StringBuilder builder)
        {
            var entity = target as Entity;
            builder.AppendLine($"IsActive: {entity.IsActive}");
            builder.AppendLine($"HP: {entity.Health}/{entity.MaxHealth}");
        }
    }

    public class PlayerDebugProfiler : IDebugProfiler
    {
        public void BuildText(GameObject target, StringBuilder builder)
        {
            var entity = target as Player;
            builder.AppendLine($"IsActive: {entity.IsActive}");
            builder.AppendLine($"HP: {entity.Health}/{entity.MaxHealth}");
            builder.AppendLine($"Current Sprite Frame: {entity.Sprite.FrameIndex}");
        }
    }
}