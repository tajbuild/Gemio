using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerShooting : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private EnergyProjectile projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Firing")]
    [SerializeField] private float fireCooldown = 0.35f;

    private SpriteRenderer spriteRenderer;
    private float nextFireTime;

    [Header("Audio")]
    [SerializeField] private AudioSource shotAudioSource;
    [SerializeField] private AudioClip shotSound;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // These references are required. Logging errors makes a missing
        // Inspector assignment easier to diagnose than silently doing nothing.
        if (projectilePrefab == null)
        {
            Debug.LogError("PlayerShooting is missing its Projectile Prefab reference.", this);
        }

        if (firePoint == null)
        {
            Debug.LogError("PlayerShooting is missing its Fire Point reference.", this);
        }

        if (shotAudioSource == null)
        {
            Debug.LogError("PlayerShooting: Shot Audio Source is not assigned.", this);
        }

        if (shotSound == null)
        {
            Debug.LogError("PlayerShooting: Shot Sound is not assigned.", this);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        // "started" makes one projectile fire when the button is first pressed,
        // instead of firing again when the button is released.
        if (context.started) Fire();
    }

    private void Fire()
    {
        
        // Ignore firing input until the energy weapon has been collected.
        if (!RunState.HasEnergyWeaponUnlocked) return;

        // Time scale is zero while paused, on game over or after level completion. 
        // OR Stop firing once levelgoal is hit.
        if (Time.timeScale == 0f || LevelGoal.isLevelComplete) return;

        // Prevent the player from firing faster than the cooldown allows.
        if (Time.time < nextFireTime) return;

        // Prevent a NullReferenceException if something was not assigned.
        if (projectilePrefab == null || firePoint == null) return;

        nextFireTime = Time.time + fireCooldown;

        // Your PlayerController uses SpriteRenderer.flipX to face left.
        Vector2 shootDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;

        // FirePoint marks the right-side position. Mirror its local X position
        // when the player faces left.
        Vector3 localSpawnPosition = firePoint.localPosition;
        localSpawnPosition.x = Mathf.Abs(localSpawnPosition.x) * shootDirection.x;

        // Convert the local firing-point position into a world position.
        Vector3 worldSpawnPosition = transform.TransformPoint(localSpawnPosition);

        EnergyProjectile projectile = Instantiate(projectilePrefab, worldSpawnPosition, Quaternion.identity);
        projectile.Launch(shootDirection);

        if (shotAudioSource != null && shotSound != null)
        {
            shotAudioSource.PlayOneShot(shotSound);
        }
    }
}