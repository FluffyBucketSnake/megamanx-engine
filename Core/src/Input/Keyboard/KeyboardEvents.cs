using Microsoft.Xna.Framework.Input;

namespace MegamanX.Input.Keyboard
{
    public record KeyboardEventArgs(IKeyboardDevice Device, Keys Key, char Character);

    public delegate void KeyboardEventHandler(KeyboardEventArgs e);
}
