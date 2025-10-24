using UnityEngine;

public class AldarionWalkingSoundController : MonoBehaviour
{
    [Header("Footstep Settings")]
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource reference for walking noises.");
            return;
        }
        
        audioSource.playOnAwake = false;
    }

    public void PlayFootstep()
    {
        if (footstepSounds == null || footstepSounds.Length == 0)
        {
            Debug.LogWarning("No footstep sounds assigned!");
            return;
        }

        AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];

        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
}
