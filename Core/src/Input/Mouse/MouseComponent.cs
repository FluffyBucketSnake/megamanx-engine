using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using XnaMouse = Microsoft.Xna.Framework.Input.Mouse;

namespace MegamanX.Input.Mouse
{
    public class MouseComponent : GameComponent, IMouseDevice
    {
        private MouseState _currentState;

        private MouseState _lastState;

        public MouseComponent(Game game) : base(game)
        {
            game.Services.AddService<IMouseDevice>(this);
        }

        public event MouseEventHandler ButtonDown;

        public event MouseEventHandler ButtonUp;

        public event MouseEventHandler Move;

        public Vector2 Position => _currentState.Position.ToVector2();

        public Vector2 Speed => Position - _lastState.Position.ToVector2();

        public bool IsButtonDown(MouseButtons button)
        {
            return _currentState.CheckButtonState(button);
        }

        public bool IsButtonPressed(MouseButtons button)
        {
            return _currentState.CheckButtonState(button) && !_lastState.CheckButtonState(button);
        }

        public bool IsButtonReleased(MouseButtons button)
        {
            return !_currentState.CheckButtonState(button) && _lastState.CheckButtonState(button);
        }

        public override void Update(GameTime gameTime)
        {
            _lastState = _currentState;
            _currentState = XnaMouse.GetState();

            CheckButtonUpdate(MouseButtons.Left);
            CheckButtonUpdate(MouseButtons.Right);
            CheckButtonUpdate(MouseButtons.Middle);

            if (Speed != Vector2.Zero)
            {
                Move?.Invoke(new MouseEventArgs(this, Position, MouseButtons.None));
            }
        }

        private void CheckButtonUpdate(MouseButtons button)
        {
            if (IsButtonPressed(button))
            {
                ButtonDown?.Invoke(new MouseEventArgs(this, Position, button));
            }
            else if (IsButtonReleased(button))
            {
                ButtonUp?.Invoke(new MouseEventArgs(this, Position, button));
            }
        }
    }
}