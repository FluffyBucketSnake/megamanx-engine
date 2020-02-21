using Microsoft.Xna.Framework.Input;

namespace MegamanX.Input.Keyboard
{
    public class KeyboardEventArgs
    {
        public IKeyboardDevice Device;

        public Keys Key;

        public char Character;

        public KeyboardEventArgs(IKeyboardDevice device, Keys key, char character)
        {
            Device = device;
            Key = key;
            Character = character;
        }
    }

    public delegate void KeyboardEventHandler(KeyboardEventArgs e);
}