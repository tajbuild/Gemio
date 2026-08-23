using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField, Min(1)] private int maxHealth = 1;

    private int currentHealth;
    private bool isDead;

    private void Awake()
    {
        // Every enemy begins with its configured maximum health.
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        // Ignore invalid damage and prevent an already-dead enemy
        // from processing additional projectile collisions.
        if (damage <= 0 || isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        // We can add a death animation, sound and points here later.
        Destroy(gameObject);
    }
}