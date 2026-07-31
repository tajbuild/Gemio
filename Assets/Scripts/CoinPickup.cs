using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;
    [SerializeField] private AudioClip pickupSound; // Drag your sound here in the Inspector

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object passing through is the Player
        if (collision.CompareTag("Player"))
        {

            // 1. Play the sound independently of this object
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position, 1f);            
            }
            
            // 2. Add points to the Game Manager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(coinValue);
            }

            // 3. Destroy the coin immediately so it can't be collected twice
            Destroy(gameObject);
        }
    }
}