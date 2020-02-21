using Microsoft.Xna.Framework;

namespace MegamanX.Input.Mouse
{
    public class MouseEventArgs
    {
        public IMouseDevice Device;

        public Vector2 Position;

        public MouseButtons Button;

        public MouseEventArgs(IMouseDevice device, Vector2 position, MouseButtons button)
        {
            Device = device;
            Position = position;
            Button = button;
        }
    }

    public delegate void MouseEventHandler(MouseEventArgs e);
}