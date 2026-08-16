using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 3;

    [Header("Invulnerability")]
    [SerializeField] private float invulnerabilityDuration = 1f;
    [SerializeField] private float flashInterval = 0.1f;

    [Header("Audio")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;

    private int currentHealth;
    private bool isInvulnerable;
    private bool isDead;
    private SpriteRenderer spriteRenderer;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    private void Start()
    {
        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || isInvulnerable || isDead || LevelGoal.isLevelComplete)
        {
            return;
        }

        currentHealth = Mathf.Max(currentHealth - amount, 0);
        UpdateHealthUI();

        if (currentHealth == 0)
        {
            Die();
            return;
        }

        if (hurtSound != null)
        {
            AudioSource.PlayClipAtPoint(hurtSound, transform.position);
        }

        StartCoroutine(InvulnerabilityRoutine());
    }

    public void Kill()
    {
        if (isDead || LevelGoal.isLevelComplete)
        {
            return;
        }

        currentHealth = 0;
        UpdateHealthUI();
        Die();
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        float elapsedTime = 0f;

        while (elapsedTime < invulnerabilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashInterval);
            elapsedTime += flashInterval;
        }

        spriteRenderer.enabled = true;
        isInvulnerable = false;
    }

    private void Die()
    {
        isDead = true;
        isInvulnerable = false;

        StopAllCoroutines();
        spriteRenderer.enabled = true;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
        }

        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.TriggerGameOver();
        }
        else
        {
            Debug.LogError("PlayerHealth could not find the GameManager.", this);
        }
    }

    private void UpdateHealthUI()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateHealthUI(currentHealth);
        }
    }
}