using UnityEngine;
using UnityEngine.SceneManagement;

public class Hazard : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private bool instantKill;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || LevelGoal.isLevelComplete)
        {
            return;
        }

        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.LogError("Hazard touched the Player, but PlayerHealth was not found.", collision);
            return;
        }

        if (instantKill)
        {
            playerHealth.Kill();
        }
        else
        {
            playerHealth.TakeDamage(damage);
        }
    }
}