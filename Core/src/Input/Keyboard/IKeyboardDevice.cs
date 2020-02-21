using Microsoft.Xna.Framework.Input;

namespace MegamanX.Input.Keyboard
{
    public interface IKeyboardDevice
    {
        event KeyboardEventHandler KeyDown;

        event KeyboardEventHandler KeyUp;

        event KeyboardEventHandler TextInput;

        bool IsKeyDown(Keys key);

        bool IsKeyPressed(Keys keys);

        bool IsKeyReleased(Keys keys);
    }
}