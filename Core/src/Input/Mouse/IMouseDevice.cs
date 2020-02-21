using Microsoft.Xna.Framework;

namespace MegamanX.Input.Mouse
{
    public interface IMouseDevice
    {
        Vector2 Position { get; }

        Vector2 Speed { get; }

        event MouseEventHandler ButtonDown;

        event MouseEventHandler ButtonUp;

        event MouseEventHandler Move;

        bool IsButtonDown(MouseButtons button);

        bool IsButtonPressed(MouseButtons button);

        bool IsButtonReleased(MouseButtons button);
    }
}