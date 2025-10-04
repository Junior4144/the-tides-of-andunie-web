using UnityEngine;

public class ProximityVolume : MonoBehaviour
{

    public float range;
    public float maxVolume;

    private AudioSource _audioSource;
    private GameObject _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _audioSource = GetComponent<AudioSource>();
    }

    void Update() =>
        _audioSource.volume = Mathf.Clamp(1 - Vector3.Distance(_player.transform.position, transform.position) / range, 0f, maxVolume);
}
