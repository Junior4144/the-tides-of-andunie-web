using UnityEngine;

public class SoundSpawnShuffler : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip[] clips;          // 🎧 Multiple sound options
    public float range = 10f;
    public float maxVolume = 1f;

    [Header("Pitch Randomization")]
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    private AudioSource _audioSource;
    private GameObject _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _audioSource = GetComponent<AudioSource>();

        if (clips.Length > 0)
        {
            // Pick a random clip each time this object is spawned
            _audioSource.clip = clips[Random.Range(0, clips.Length)];
        }
        else
        {
            Debug.LogWarning("No audio clips assigned to ProximityVolume on " + gameObject.name);
            return;
        }

        // Random pitch for variation
        _audioSource.pitch = Random.Range(minPitch, maxPitch);

        // Play the chosen clip
        _audioSource.Play();

        // Destroy after clip finishes (if not looping)
        if (!_audioSource.loop)
            Destroy(gameObject, _audioSource.clip.length);
    }

    void Update()
    {
        if (_player == null || _audioSource.clip == null) return;

        float distance = Vector3.Distance(_player.transform.position, transform.position);
        float volume = 1 - (distance / range);
        _audioSource.volume = Mathf.Clamp(volume, 0f, maxVolume);
    }
}