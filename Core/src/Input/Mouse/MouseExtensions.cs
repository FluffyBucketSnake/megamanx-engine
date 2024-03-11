using Microsoft.Xna.Framework.Input;

namespace MegamanX.Input.Mouse
{
    public static class MouseExtensions
    {
        public static ButtonState GetButtonState(this MouseState state, MouseButtons button)
        {
            return button switch
            {
                MouseButtons.Left => state.LeftButton,
                MouseButtons.Right => state.RightButton,
                MouseButtons.Middle => state.MiddleButton,
                _ => ButtonState.Released,
            };
        }

        public static bool CheckButtonState(this MouseState state, MouseButtons button)
        {
            return state.GetButtonState(button) == ButtonState.Pressed;
        }
    }
}
