using Microsoft.Xna.Framework;

namespace MegamanX.Input.Mouse
{
    public record MouseEventArgs(IMouseDevice Device, Vector2 Position, MouseButtons Button) { }
    public delegate void MouseEventHandler(MouseEventArgs e);
}
