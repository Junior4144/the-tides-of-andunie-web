using UnityEngine;

public class ProximityVolume : MonoBehaviour
{
    public float range;
    public float maxVolume;

    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    private AudioSource _audioSource;
    private GameObject _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _audioSource = GetComponent<AudioSource>();

        _audioSource.pitch = Random.Range(minPitch, maxPitch);

        if (!_audioSource.loop)
            Destroy(gameObject, _audioSource.clip.length);
    }

    void Update()
    {
        if (_player == null) return;

        _audioSource.volume = Mathf.Clamp(1 - Vector3.Distance(_player.transform.position, transform.position) / range, 0f, maxVolume);
    }
}
