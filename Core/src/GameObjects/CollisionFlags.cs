using System;
using System.Collections.Generic;
using System.Text;

namespace MegamanX.GameObjects
{
    [Flags]
    public enum CollisionFlags : ushort
    {
        None = 0x0000,
        World = 0x0001,
        Player = 0x0002,
        PlayerProjectile = 0x0004,
        Enemy = 0x0008,
        EnemyProjectile = 0x0010,
        All = 0xFFFF
    }
}
