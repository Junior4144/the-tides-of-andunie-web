using UnityEngine;

public class PingSoundController : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("PingSoundController: No AudioSource found on this object!");
            Destroy(gameObject);
            return;
        }

        // Play the sound
        audioSource.Play();

        // Destroy after clip finishes
        Destroy(gameObject, audioSource.clip.length);
    }
}