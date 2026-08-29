using UnityEngine;
using UnityEngine.SceneManagement;

public class PatrolEnemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Transform wallCheckPoint;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool movingRight = false;

    [Header("Combat Settings")]
    [SerializeField] private float bounceForce = 8f;
    [SerializeField] private float topCollisionOffset = 0.5f;

    [SerializeField] private AudioClip squishSound; // Drag your sound here in the Inspector
    [SerializeField] private AudioClip deathSound;  // Sound for when the enemy hits you

    [SerializeField] private int contactDamage = 1;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Move the enemy continuously in its current direction
        float currentSpeed = movingRight ? moveSpeed : -moveSpeed;

        // Patrol enemies never intentionally move upward.
        // Remove upward velocity caused by collisions while preserving gravity/falling.
        float verticalVelocity = Mathf.Min(rb.linearVelocity.y, 0f);

        rb.linearVelocity = new Vector2(currentSpeed, verticalVelocity);
        // Check if there is a wall or an edge ahead
        bool hitWall = Physics2D.OverlapCircle(wallCheckPoint.position, checkRadius, groundLayer);
        
        // If we hit a wall/edge, flip direction
        if (hitWall)
        {
            movingRight = !movingRight;
            FlipSprite();
        }
    }

    private void FlipSprite()
    {
        // Turn the visual sprite around
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object colliding with the enemy is the Player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Calculate if the player is physically above the enemy's center point
            bool hitFromAbove = collision.transform.position.y > transform.position.y + topCollisionOffset;

            if (hitFromAbove)
            {
                // STOMP SUCCESSFUL
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    // Reset the player's downward velocity, then apply the upward bounce force
                    playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceForce);
                }

                // 1. Squish the enemy to half its height
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 0.5f, transform.localScale.z);
                
                // 2. Disable the collider so the player doesn't bounce on it twice or take damage
                GetComponent<Collider2D>().enabled = false;
                
                // 3. Disable this script so the enemy stops running/updating
                this.enabled = false;

                AudioSource.PlayClipAtPoint(squishSound, Camera.main.transform.position, 1f);

                // 4. Destroy the object after a 0.2 second delay so the squish is visible
                Destroy(gameObject, 0.2f);
            }
            else
            {
                if (LevelGoal.isLevelComplete) return;
                
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(contactDamage);
                }               
            }
        }
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") ||
            LevelGoal.isLevelComplete)
        {
            return;
        }

        PlayerHealth playerHealth =
            collision.gameObject.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            // PlayerHealth ignores this call during its invulnerability period.
            // Once invulnerability expires, continued contact causes damage again.
            playerHealth.TakeDamage(contactDamage);
        }
    }
    
    private void OnDrawGizmos()
    {
        // Draws a little red circle in the editor scene view to help you see the wall detector
        if (wallCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(wallCheckPoint.position, checkRadius);
        }
    }
}