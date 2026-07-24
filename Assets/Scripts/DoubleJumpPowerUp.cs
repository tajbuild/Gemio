using UnityEngine;

public class DoubleJumpPowerUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            
            if (player != null)
            {
                // Unlock the double jump
                player.hasDoubleJumpUnlocked = true;
                
                // Destroy the power-up object
                Destroy(gameObject);
            }
        }
    }
}