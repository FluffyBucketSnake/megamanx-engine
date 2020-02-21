using Microsoft.Xna.Framework;

namespace MegamanX.Physics
{
    public class PositionChangedArgs
    {
        public Vector2 NewPosition;
        
        public Vector2 OldPosition;

        public PositionChangedArgs(Vector2 oldPosition, Vector2 newPosition)
        {
            OldPosition = oldPosition;
            NewPosition = newPosition;
        }
    }
}