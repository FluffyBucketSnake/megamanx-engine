using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using XnaKeyboard = Microsoft.Xna.Framework.Input.Keyboard;

namespace MegamanX.Input.Keyboard
{
    public class KeyboardComponent : GameComponent, IKeyboardDevice
    {
        KeyboardState _currentState;
        KeyboardState _lastState;

        public KeyboardComponent(Game game) : base(game)
        {
            game.Window.TextInput += (sender, e) =>
            {
                TextInput?.Invoke(new KeyboardEventArgs(this, e.Key, e.Character));
            };

            game.Services.AddService<IKeyboardDevice>(this);
        }

        public event KeyboardEventHandler KeyDown;

        public event KeyboardEventHandler KeyUp;

        public event KeyboardEventHandler TextInput;

        public bool IsKeyDown(Keys key)
        {
            return _currentState.IsKeyDown(key);
        }

        public bool IsKeyPressed(Keys key)
        {
            return _currentState.IsKeyDown(key) && _lastState.IsKeyUp(key);
        }

        public bool IsKeyReleased(Keys key)
        {
            return _currentState.IsKeyUp(key) && _lastState.IsKeyDown(key);
        }

        public override void Update(GameTime gameTime)
        {
            _lastState = _currentState;
            _currentState = XnaKeyboard.GetState();
            
            // TODO: Map keys to characters.
            foreach (var key in KeyboardHelper.Keys)
            {
                if (IsKeyPressed(key))
                {
                    KeyDown?.Invoke(new KeyboardEventArgs(this, key, '\0'));
                }
                else if (IsKeyReleased(key))
                {
                    KeyUp?.Invoke(new KeyboardEventArgs(this, key, '\0'));
                }
            }
        }
    }
}