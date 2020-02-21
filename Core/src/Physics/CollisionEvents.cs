using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MegamanX.Physics
{
    public delegate void BodyCollisionEventHandler(BodyCollisionInfo info);

    public class BodyCollisionInfo
    {
        public GameTime GameTime;

        public Vector2 Penetration;

        public PhysicBody CollidingBody;
    }

    public delegate void TilemapCollisionEventHandler(TileMapCollisionInfo info);

    public class TileMapCollisionInfo
    {
        public GameTime GameTime;

        public Vector2 Penetration;
    }
}