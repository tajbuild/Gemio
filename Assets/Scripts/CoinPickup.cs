using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object passing through is the Player
        if (collision.CompareTag("Player"))
        {
            // Tell our global manager to add points
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(coinValue);
            }

            // Destroy the coin immediately so it can't be collected twice
            Destroy(gameObject);
        }
    }
}