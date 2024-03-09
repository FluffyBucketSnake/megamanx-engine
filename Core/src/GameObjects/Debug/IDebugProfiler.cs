using System.Text;

namespace MegamanX.GameObjects.Debug
{
    public interface IDebugProfiler
    {
        void BuildText(LegacyGameObject target, StringBuilder builder);
    }
}
