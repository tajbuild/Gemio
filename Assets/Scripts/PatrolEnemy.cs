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

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Move the enemy continuously in its current direction
        float currentSpeed = movingRight ? moveSpeed : -moveSpeed;
        rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);

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

                    // 4. Destroy the object after a 0.2 second delay so the squish is visible
                    Destroy(gameObject, 0.2f);
                }
                else
                {
                    // PLAYER HIT THE SIDE OR BOTTOM
                    // Respect the global win state we set up earlier!
                    if (LevelGoal.isLevelComplete) return;

                    // Restart the current level
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }
    
/*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the enemy bumps into the player, reset the level
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit by enemy! Restarting...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
*/
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