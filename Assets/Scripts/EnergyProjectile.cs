using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class EnergyProjectile : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifetime = 3f;

    [Header("Impact")]
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] private GameObject explosionEffectPrefab;

    [SerializeField, Min(1)] private int damage = 1;

    private Rigidbody2D rb;
    private bool hasExploded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Destroy projectiles that miss everything so they do not
        // continue travelling forever or accumulate in memory.
        Destroy(gameObject, lifetime);
    }

    public void Launch(Vector2 direction)
    {
        // Normalize makes the direction exactly one unit long,
        // so speed remains consistent when firing left or right.
        rb.linearVelocity = direction.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Prevent one projectile from damaging multiple colliders during the same physics frame.
        if (hasExploded) return;

        // Ignore objects whose layers are not selected under Hit Layers.
        if ((hitLayers.value & (1 << other.gameObject.layer)) == 0) return;

        // Look for EnemyHealth on the collider or its parent.
        // GetComponentInParent also works if the enemy's collider
        // is located on a child object.
        EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }

        // The projectile also explodes when hitting ordinary terrain,
        // even though terrain does not have an EnemyHealth component.
        Explode();
    }

    private void Explode()
    {
        // A projectile can sometimes receive multiple physics callbacks
        // during the same frame, so only allow one explosion.
        if (hasExploded) return;

        hasExploded = true;

        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}