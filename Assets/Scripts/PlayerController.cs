using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;
    
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.8f, 0.1f);
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;
    //Declaring animator:
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    [Header("Power-Ups")]
    public bool hasDoubleJumpUnlocked = false; // Unlocks when the item is picked up
    private bool canDoubleJump; // Tracks if the player has already used their mid-air jump

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // Grab the Animator component 
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0f, groundLayer);

        // Reset double jump when grounded
        if (isGrounded)
        {
            canDoubleJump = true;
        }

        // Update the animator parameter based on movement input
        if (anim != null)
        {
            bool isMoving = Mathf.Abs(horizontalInput) > 0.1f;
            anim.SetBool("isRunning", isMoving);
            // Pass jump and fall parameters to the animator
            anim.SetBool("isGrounded", isGrounded);
            anim.SetFloat("yVelocity", rb.linearVelocity.y);

            // Flip the character sprite based on movement direction
            FlipSprite();
        }
    }

    private void FlipSprite()
{
    // If moving right, face right (flipX = false)
    if (horizontalInput > 0.1f)
    {
        spriteRenderer.flipX = false;
    }
    // If moving left, face left (flipX = true)
    else if (horizontalInput < -0.1f)
    {
        spriteRenderer.flipX = true;
    }
}

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>().x;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
            {
                if (isGrounded)
                {
                    // First jump from the ground
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                }
                else if (hasDoubleJumpUnlocked && canDoubleJump)
                {
                    // Mid-air double jump
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                    canDoubleJump = false; // Consume the second jump
                }
            }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
        }
    }
}