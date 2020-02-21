using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace MegamanX.Input.Keyboard
{
    public static class KeyboardHelper
    {
        public readonly static IEnumerable<Keys> Keys;

        static KeyboardHelper()
        {
            Keys = System.Enum.GetValues(typeof(Keys)).Cast<Keys>();
        }
    }
}