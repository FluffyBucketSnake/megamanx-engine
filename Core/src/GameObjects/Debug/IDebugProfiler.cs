using System.Text;

namespace MegamanX.GameObjects.Debug
{
    public interface IDebugProfiler
    {
        void BuildText(GameObject target, StringBuilder builder);
    }
}