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

    private Animator animator;

    [Header("Death Animation")]
    [SerializeField, Min(0)] private int deathFlashCount = 3;
    [SerializeField, Min(0.01f)] private float deathFlashInterval = 0.08f;

    private void Awake()
    {
        currentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        bossCollider = GetComponent<Collider2D>();
        bossController = GetComponent<BossController>();
        animator = GetComponent<Animator>();


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

        if (animator == null)
        {
            Debug.LogError("BossHealth could not find the Animator.", this);
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

        // Cancel an unfinished normal-hit flash.
        StopAllCoroutines();

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

        // Stop movement and contact damage immediately while allowing
        // the sprite animation to continue.
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

        // Override the running state with the death animation.
        if (animator != null)
        {
            animator.SetBool("isMoving", false);
            animator.SetTrigger("Die");
        }

        PlaySound(deathSound);

        StartCoroutine(DeathRoutine());
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

    private IEnumerator DeathRoutine()
    {
        float flashingDuration = deathFlashCount * deathFlashInterval * 2f;

        // Pulse between the configured hit color and the original color.
        for (int i = 0; i < deathFlashCount; i++)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = hitFlashColor;
            }

            yield return new WaitForSeconds(deathFlashInterval);

            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }

            yield return new WaitForSeconds(deathFlashInterval);
        }

        // Allow the death animation to finish before the explosion.
        float remainingDelay = Mathf.Max(0f, destroyDelay - flashingDuration);

        yield return new WaitForSeconds(remainingDelay);

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

        if (deathEffectPrefab != null)
        {
            GameObject deathEffect = Instantiate(
                deathEffectPrefab,
                transform.position,
                Quaternion.identity
            );

            deathEffect.transform.localScale *= deathEffectScale;
        }

        // Open the arena and reveal the goal only after the sequence finishes.
        onBossDefeated?.Invoke();

        Destroy(gameObject);
    }
}