using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundSpawnShuffler : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] AudioClip[] _clips;
    [SerializeField] float _range = 10f;
    [SerializeField] float _maxVolume = 1f;
    [Header("Pitch Randomization")]
    [SerializeField] float _minPitch = 1f;
    [SerializeField] float _maxPitch = 1f;

    [Header("Playback Settings")]
    [SerializeField] bool _playOnStart = true;
    private Transform _player;

    AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _player = PlayerManager.Instance.GetPlayerTransform();

        if (_playOnStart)
            Play();
    }

    void Update()
    {
        if (_player == null || _audioSource.clip == null) return;

        float distance = Vector3.Distance(_player.position, transform.position);
        float volume = 1 - (distance / _range);
        _audioSource.volume = Mathf.Clamp(volume, 0f, _maxVolume);
    }

    public void Play()
    {
        if (_clips.Length == 0)
        {
            Debug.LogWarning($"[SoundSpawnShuffler] No clips assigned {gameObject.name}");
            return;
        }

        _audioSource.clip = _clips[Random.Range(0, _clips.Length)];
        _audioSource.pitch = Random.Range(_minPitch, _maxPitch);
        _audioSource.Play();

        Destroy(gameObject, _audioSource.clip.length);
    }
}