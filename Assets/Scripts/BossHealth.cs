using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField, Min(1)] private int maxHealth = 10;

    [Header("Health Bar")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject healthBarObject;

    [Header("Hit Feedback")]
    [SerializeField] private Color hitFlashColor = new Color(1f, 0.35f, 0.35f);
    [SerializeField] private float hitFlashDuration = 0.1f;
    [SerializeField] private AudioClip hitSound;

    [Header("Death Feedback")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private GameObject deathEffectPrefab;
    [SerializeField] private float deathEffectScale = 1.5f;
    [SerializeField] private float destroyDelay = 0.75f;

    [Header("Events")]
    [SerializeField] private UnityEvent onBossDefeated;

    private int currentHealth;
    private bool isDead;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Collider2D bossCollider;
    private BossController bossController;

    private Color originalColor;
    private Coroutine hitFlashCoroutine;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private bool isVulnerable = true;

    private void Awake()
    {
        currentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        bossCollider = GetComponent<Collider2D>();
        bossController = GetComponent<BossController>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        if (healthSlider == null)
        {
            Debug.LogError("BossHealth: Health Slider is not assigned.", this);
        }
        else
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (healthBarObject == null)
        {
            Debug.LogError("BossHealth: Health Bar Object is not assigned.", this);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isVulnerable || damage <= 0 || isDead) return;

        currentHealth = Mathf.Max(currentHealth - damage, 0);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth == 0)
        {
            Die();
            return;
        }

        PlayHitFeedback();
    }

    private void PlayHitFeedback()
    {
        if (hitSound != null)
        {
            PlaySound(hitSound);
        }

        if (spriteRenderer != null)
        {
            // Restart the flash if another projectile hits before it finishes.
            if (hitFlashCoroutine != null)
            {
                StopCoroutine(hitFlashCoroutine);
            }

            hitFlashCoroutine = StartCoroutine(HitFlashRoutine());
        }
    }

    private IEnumerator HitFlashRoutine()
    {
        spriteRenderer.color = hitFlashColor;

        yield return new WaitForSeconds(hitFlashDuration);

        spriteRenderer.color = originalColor;
        hitFlashCoroutine = null;
    }

    private void Die()
    {
        isDead = true;

        // Stop any unfinished hit flash and restore the sprite colour.
        StopAllCoroutines();

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

        // Stop the boss immediately so it cannot move or damage the player
        // while its death effect is playing.
        if (bossController != null)
        {
            bossController.enabled = false;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        if (bossCollider != null)
        {
            bossCollider.enabled = false;
        }

        if (healthBarObject != null)
        {
            healthBarObject.SetActive(false);
        }

        if (deathSound != null)
        {
            PlaySound(deathSound);
        }

        if (deathEffectPrefab != null)
        {
            GameObject deathEffect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            deathEffect.transform.localScale *= deathEffectScale;
        }

        // Notify the BossLevel scene that the boss was defeated.
        onBossDefeated?.Invoke();

        Destroy(gameObject, destroyDelay);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip == null) return;

        // Play at the camera/AudioListener position to avoid 3D distance attenuation.
        Vector3 soundPosition = Camera.main != null ? Camera.main.transform.position : transform.position;
        AudioSource.PlayClipAtPoint(clip, soundPosition, 1f);
    }

    public void SetVulnerable(bool vulnerable)
    {
        isVulnerable = vulnerable;
    }
}