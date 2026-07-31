using UnityEngine;

public class DoubleJumpPowerUp : MonoBehaviour
{
    
    [SerializeField] private AudioClip pickupSound; // Drag your sound here in the Inspector

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            
            if (player != null)
            {
                // Unlock the double jump
                player.hasDoubleJumpUnlocked = true;
                
                // Play Pickup Sound 
                if (pickupSound != null)
                {
                    AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position, 1f);            
                }

                // Destroy the power-up object
                Destroy(gameObject);
            }
        }
    }
}