using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class BossController : MonoBehaviour
{
    [Header("Normal Movement")]
    [SerializeField] private float patrolSpeed = 2f;

    // The boss can travel this far in either direction from its starting position.
    [SerializeField] private float patrolHalfWidth = 4f;

    [Header("Charge")]
    [SerializeField] private float chargeSpeed = 7f;
    [SerializeField] private float chargeInterval = 3f;
    [SerializeField] private float chargeDuration = 0.75f;

    [Header("Damage")]
    [SerializeField, Min(1)] private int contactDamage = 1;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Transform player;

    private float leftBoundary;
    private float rightBoundary;
    private float direction = 1f;

    private float nextChargeTime;
    private float chargeEndTime;
    private bool isCharging;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Calculate the movement boundaries from the boss's starting position.
        leftBoundary = transform.position.x - patrolHalfWidth;
        rightBoundary = transform.position.x + patrolHalfWidth;

        // The Player is a scene object, so the reusable Boss prefab finds it
        // when the scene starts instead of storing a scene reference.
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("BossController could not find the Player.", this);
        }

        nextChargeTime = Time.time + chargeInterval;
    }

    private void FixedUpdate()
    {
        if (LevelGoal.isLevelComplete)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        HandleArenaBoundaries();

        // Begin another charge when its cooldown has finished.
        if (!isCharging && player != null && Time.time >= nextChargeTime)
        {
            BeginCharge();
        }

        // Return to normal movement after the charge duration.
        if (isCharging && Time.time >= chargeEndTime)
        {
            FinishCharge();
        }

        float currentSpeed = isCharging ? chargeSpeed : patrolSpeed;

        rb.linearVelocity = new Vector2(direction * currentSpeed, rb.linearVelocity.y);

        // Flip only the sprite so the world-space health bar stays readable.
        spriteRenderer.flipX = direction < 0f;
    }

    private void BeginCharge()
    {
        isCharging = true;
        chargeEndTime = Time.time + chargeDuration;

        // Choose the player's current direction when the charge begins.
        direction = player.position.x >= transform.position.x ? 1f : -1f;
    }

    private void FinishCharge()
    {
        isCharging = false;
        nextChargeTime = Time.time + chargeInterval;
    }

    private void HandleArenaBoundaries()
    {
        if (transform.position.x <= leftBoundary)
        {
            direction = 1f;

            if (isCharging)
            {
                FinishCharge();
            }
        }
        else if (transform.position.x >= rightBoundary)
        {
            direction = -1f;

            if (isCharging)
            {
                FinishCharge();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") || LevelGoal.isLevelComplete) return;

        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            // PlayerHealth handles invulnerability and death internally.
            playerHealth.TakeDamage(contactDamage);
        }
    }
}