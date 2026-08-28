using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BossArenaController : MonoBehaviour
{
    [Header("Boss")]
    [SerializeField] private BossController bossController;
    [SerializeField] private GameObject bossHealthCanvas;
    [SerializeField] private BossHealth bossHealth;

    [Header("Arena")]
    [SerializeField] private GameObject entranceGate;

    [Header("Level Goal")]
    [SerializeField] private Collider2D levelGoalCollider;
    [SerializeField] private SpriteRenderer levelGoalRenderer;

    private BoxCollider2D fightTrigger;
    private bool fightStarted;
    private bool bossDefeated;

    private void Awake()
    {
        fightTrigger = GetComponent<BoxCollider2D>();

        if (bossController == null)
        {
            Debug.LogError("BossArenaController: Boss Controller is not assigned.", this);
        }
        else
        {
            // The boss remains stationary until the player enters.
            bossController.enabled = false;
        }

        if (bossHealthCanvas == null)
        {
            Debug.LogError("BossArenaController: Boss Health Canvas is not assigned.", this);
        }
        else
        {
            bossHealthCanvas.SetActive(false);
        }

        if (entranceGate == null)
        {
            Debug.LogError("BossArenaController: Entrance Gate is not assigned.", this);
        }
        else
        {
            // The entrance begins open.
            entranceGate.SetActive(false);
        }

        if (levelGoalCollider == null)
        {
            Debug.LogError("BossArenaController: Level Goal Collider is not assigned.", this);
        }
        else
        {
            levelGoalCollider.enabled = false;
        }

        if (levelGoalRenderer == null)
        {
            Debug.LogError("BossArenaController: Level Goal Renderer is not assigned.", this);
        }
        else
        {
            levelGoalRenderer.enabled = false;
        }
        if (bossHealth == null)
        {
            Debug.LogError("BossArenaController: Boss Health is not assigned.", this);
        }
        else
        {
            // Prevent the player from damaging the boss before entering the arena.
            bossHealth.SetVulnerable(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || fightStarted) return;

        fightStarted = true;

        // Close the entrance behind the player.
        if (entranceGate != null)
        {
            entranceGate.SetActive(true);
        }

        // Reveal the boss's world-space health bar.
        if (bossHealthCanvas != null)
        {
            bossHealthCanvas.SetActive(true);
        }

        if (bossHealth != null)
        {
            bossHealth.SetVulnerable(true);
        }

        // Begin boss movement and charging.
        if (bossController != null)
        {
            bossController.enabled = true;
        }

        // The encounter only needs to start once.
        fightTrigger.enabled = false;
    }

    public void HandleBossDefeated()
    {
        if (bossDefeated) return;

        bossDefeated = true;

        // Reopen the arena entrance.
        if (entranceGate != null)
        {
            entranceGate.SetActive(false);
        }

        // Reveal and enable the trophy/goal.
        if (levelGoalRenderer != null)
        {
            levelGoalRenderer.enabled = true;
        }

        if (levelGoalCollider != null)
        {
            levelGoalCollider.enabled = true;
        }
    }
}