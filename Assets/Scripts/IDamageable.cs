public interface IDamageable
{
    // Anything that can receive projectile damage must provide this method.
    void TakeDamage(int damage);
}