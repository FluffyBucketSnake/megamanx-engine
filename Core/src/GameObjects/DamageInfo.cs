namespace MegamanX.GameObjects
{
    public class DamageInfo
    {
        public LegacyGameObject Source;

        public int Damage;

        public bool IgnoreInvincibility;

        public bool DisableKnockback;

        public Direction2D Direction;

        public DamageInfo(LegacyGameObject source, int damage, Direction2D direction, bool ignoreInvincibility = false,
        bool disableKnockback = false)
        {
            Source = source;
            Damage = damage;
            IgnoreInvincibility = ignoreInvincibility;
            DisableKnockback = disableKnockback;
            Direction = direction;
        }
    }
}
