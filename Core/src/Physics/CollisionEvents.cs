using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MegamanX.Physics
{
    public record BodyCollisionInfo(GameTime GameTime, Vector2 Penetration, PhysicBody CollidingBody);
    public record TileMapCollisionInfo(GameTime GameTime, Vector2 Penetration);

    public delegate void BodyCollisionEventHandler(BodyCollisionInfo info);
    public delegate void TilemapCollisionEventHandler(TileMapCollisionInfo info);
}
