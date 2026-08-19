using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.7f;

    private void Start()
    {
        // Remove the effect after its animation has finished.
        // This prevents finished explosions from remaining in the scene.
        Destroy(gameObject, lifetime);
    }
}