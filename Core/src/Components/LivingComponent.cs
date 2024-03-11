using MegamanX.Math;

namespace MegamanX.Components
{
    public record DamageInfo(Entity Source, int Damage, bool IgnoreInvincibility, bool DisableKnockback, Direction2D Direction) { }

    public struct HealthChangeArgs(int oldHealth, int newHealth)
    {
        public readonly int OldHealth => oldHealth;
        public readonly int NewHealth => newHealth;
    }

    public class LivingComponent(Entity entity, int maxHealth, int health = 1) : IComponent
    {
        public int Health
        {
            get => health;
            set
            {
                //Call callback function.
                HealthChangeArgs e = new(health, value);
                HealthChanged?.Invoke(this, entity, e);

                //Check if life is equal or bellow zero.
                if (e.NewHealth <= 0)
                {
                    health = 0;
                    Died?.Invoke(this, entity);
                }
                else
                {
                    health = e.NewHealth;
                }
            }
        }

        public int MaxHealth => maxHealth;

        public int InvincibilityTime { get; set; }

        public event OwnedComponentEvent<LivingComponent, HealthChangeArgs>? HealthChanged;

        public event OwnedComponentEvent<LivingComponent, DamageInfo>? Damaged;

        public event OwnedComponentEvent<LivingComponent>? Died;

        public void Damage(DamageInfo info)
        {
            if (InvincibilityTime > 0 && !info.IgnoreInvincibility)
            {
                return;
            }

            Damaged?.Invoke(this, entity, info);
            Health -= info.Damage;
        }
    }
}
