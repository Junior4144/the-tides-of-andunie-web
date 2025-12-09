using UnityEngine;

public class AldarionWalkingSoundController : MonoBehaviour
{
    [Header("Footstep Settings")]
    [SerializeField] private AudioClip[] _footstepSounds;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _footstepInterval = 0.5f;

    private PlayerController _playerController;
    private float _footstepTimer;

    private void Start()
    {
        if (_audioSource == null)
        {
            Debug.LogError("No AudioSource reference for walking noises.");
            return;
        }

        _audioSource.playOnAwake = false;
        _playerController = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        if (_playerController == null) return;

        if (!_playerController.isActiveAndEnabled || !_playerController.IsWalking)
        {
            _footstepTimer = 0f;
            return;
        }

        _footstepTimer -= Time.deltaTime;

        if (_footstepTimer <= 0f)
        {
            PlayFootstep();
            _footstepTimer = _footstepInterval;
        }
    }

    public void PlayFootstep()
    {
        if (_footstepSounds == null || _footstepSounds.Length == 0)
        {
            Debug.LogWarning("No footstep sounds assigned!");
            return;
        }

        AudioClip clip = _footstepSounds[Random.Range(0, _footstepSounds.Length)];

        if (clip != null)
            _audioSource.PlayOneShot(clip);
    }
}
