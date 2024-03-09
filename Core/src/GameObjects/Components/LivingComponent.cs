namespace MegamanX.GameObjects.Components
{
    public struct HealthChangeArgs(int oldHealth, int newHealth)
    {
        public readonly int OldHealth => oldHealth;
        public readonly int NewHealth => newHealth;
    }

    public class LivingComponent(GameObject parent, int maxHealth, int health = 1) : IComponent
    {
        public int Health
        {
            get => health;
            set
            {
                //Call callback function.
                HealthChangeArgs e = new HealthChangeArgs(health, value);
                HealthChanged(e);

                //Check if life is equal or bellow zero.
                if (e.NewHealth <= 0)
                {
                    health = 0;
                    Died(null);
                }
                else
                {
                    health = e.NewHealth;
                }
            }
        }

        public int MaxHealth => maxHealth;

        public int InvincibilityTime { get; set; }

        public event OwnedComponentEvent<LivingComponent, HealthChangeArgs> HealthChanged;

        public event OwnedComponentEvent<LivingComponent, HealthChangeArgs> Damaged;

        public event OwnedComponentEvent<LivingComponent, HealthChangeArgs> Died;

        public void Damage(DamageInfo info)
        {
            if (InvincibilityTime > 0 && !info.IgnoreInvincibility)
            {
                return;
            }

            Damaged(info);
            Health -= info.Damage;
        }
    }
}
