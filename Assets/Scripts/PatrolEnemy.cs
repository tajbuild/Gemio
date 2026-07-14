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
        // If the enemy bumps into the player, reset the level
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit by enemy! Restarting...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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