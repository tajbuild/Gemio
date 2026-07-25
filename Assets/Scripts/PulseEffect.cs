using UnityEngine;

public class PulseEffect : MonoBehaviour
{
    private Vector3 startScale;

    void Start()
    {
        // Store the initial size so it pulses relative to the Inspector scale
        startScale = transform.localScale;
    }

    void Update()
    {
        // Smoothly scale up and down using a sine wave based on time
        transform.localScale = startScale * (1f + Mathf.Sin(Time.time * 5f) * 0.2f);
    }
}