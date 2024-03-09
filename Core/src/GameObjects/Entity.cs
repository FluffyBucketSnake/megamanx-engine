using Microsoft.Xna.Framework;

namespace MegamanX.GameObjects
{
    public class HealthChangeArgs
    {
        public int OldHealth;
        public int NewHealth;
        public bool KeepAlive;

        public HealthChangeArgs(int oldHealth, int newHealth, bool keepAlive = false)
        {
            OldHealth = oldHealth;
            NewHealth = newHealth;
            KeepAlive = keepAlive;
        }
    }

    public abstract class Entity : LegacyGameObject
    {
        private int health = 1;

        public int Health
        {
            get => health;
            set
            {
                //Call callback function.
                var e = new HealthChangeArgs(health, value);
                OnHealthChange(e);

                //Check if life is equal or bellow zero.
                if (e.NewHealth <= 0)
                {
                    health = 0;
                    if (!e.KeepAlive)
                    {
                        OnDeath(null);
                    }
                }
                else
                {
                    health = e.NewHealth;
                }
            }
        }

        public int MaxHealth { get; set; } = 1;

        public int InvincibilityTime { get; set; }

        public override void Update(GameTime gameTime)
        {
            //Reduce invencibility time.
            if (InvincibilityTime > 0)
            {
                InvincibilityTime -= gameTime.ElapsedGameTime.Milliseconds;
            }
            else
            {
                InvincibilityTime = 0;
            }
        }

        public void Damage(DamageInfo info)
        {
            //Check invincibility.
            if (InvincibilityTime > 0 && !info.IgnoreInvincibility)
            {
                return;
            }

            //Call callback functions.
            OnDamage(info);
            var e = new HealthChangeArgs(health, health - info.Damage);
            OnHealthChange(e);

            //Check if life is equal or bellow zero.
            if (e.NewHealth <= 0)
            {
                health = 0;
                if (!e.KeepAlive)
                {
                    OnDeath(info.Source);
                }
            }
            else
            {
                health = e.NewHealth;
            }
        }

        public void Kill(object source)
        {
            OnDeath(source);
        }

        protected virtual void OnHealthChange(HealthChangeArgs e) { }

        protected virtual void OnDamage(DamageInfo info) { }

        protected virtual void OnDeath(object source)
        {
            Destroy();
        }
    }
}
