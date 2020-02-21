using Microsoft.Xna.Framework.Input;

namespace MegamanX.Input.Mouse
{
    public static class MouseExtensions
    {
        public static ButtonState GetButtonState(this MouseState state, MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:
                return state.LeftButton;

                case MouseButtons.Right:
                return state.RightButton;

                case MouseButtons.Middle:
                return state.MiddleButton;

                default:
                return ButtonState.Released;
            }
        }

        public static bool CheckButtonState(this MouseState state, MouseButtons button)
        {
            return (state.GetButtonState(button) == ButtonState.Pressed);
        }
    }
}