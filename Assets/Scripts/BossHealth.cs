using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour, IDamageable
{
    [SerializeField, Min(1)] private int maxHealth = 10;

    private int currentHealth;
    private bool isDead;

    // These will allow the health bar to read the boss's health later.
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    [Header("Health Bar")]
    [SerializeField] private Slider healthSlider;

    private void Awake()
    {
        currentHealth = maxHealth;

        if (healthSlider == null)
        {
            Debug.LogError("BossHealth: Health Slider is not assigned.", this);
            return;
        }

        // Configure the bar from the boss's actual health values.
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        // Ignore invalid damage and additional hits after death.
        if (damage <= 0 || isDead) return;

        currentHealth = Mathf.Max(currentHealth - damage, 0);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
        // Temporary diagnostic message until we create the health bar.
        Debug.Log("Boss health: " + currentHealth + "/" + maxHealth, this);

        if (currentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        // Hide the empty health bar immediately.
        if (healthSlider != null)
        {
            healthSlider.gameObject.SetActive(false);
        }

        Debug.Log("Boss defeated!", this);

        // We will replace this with the complete death sequence later.
        Destroy(gameObject);
    }
}