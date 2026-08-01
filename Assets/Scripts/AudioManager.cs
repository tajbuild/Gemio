using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // The Singleton instance
    public static AudioManager Instance { get; private set; }

    private AudioSource bgmSource;

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            // Grab the AudioSource component attached to this same object
            bgmSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // A public method anyone can call to stop the music
    public void StopMusic()
    {
        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }
    }
}