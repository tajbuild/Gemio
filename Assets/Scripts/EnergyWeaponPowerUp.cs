using UnityEngine;

public class EnergyWeaponPowerUp : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSound;

    private void Start()
    {
        // Remove this power-up if it was already collected earlier in the run.
        // This prevents it from reappearing after retrying the level.
        if (RunState.HasEnergyWeaponUnlocked)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // Save the weapon unlock for the remainder of the run.
        RunState.UnlockEnergyWeapon();

        // Reveal the Android Fire button immediately.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EnableFireButtonUI();
        }

        if (pickupSound != null)
        {
            // Playing at the camera position ensures the 2D pickup sound
            // remains clearly audible.
            Vector3 soundPosition = Camera.main != null ? Camera.main.transform.position : transform.position;
            AudioSource.PlayClipAtPoint(pickupSound, soundPosition, 1f);
        }

        // We will add the Fire button UI update in the next step.
        Destroy(gameObject);
    }
}