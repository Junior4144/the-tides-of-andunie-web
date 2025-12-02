using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PeacefulVillageSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSrc;

    [Header("Clips")]
    [SerializeField] private AudioClip[] _peacefulMusicClips;

    void Awake()
    {
        PlayRandomClip(_peacefulMusicClips);
    }

    private void PlayRandomClip(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
        {
            Debug.LogWarning("List of AudioClips in Peaceful Village Sound Controller was not assigned!");
            return;
        }
        if (_audioSrc == null)
        {
            Debug.LogWarning("Audio source in Peaceful Village Sound Controller was not assigned!");
            return;
        }
        _audioSrc.clip = _peacefulMusicClips[Random.Range(0, clips.Length)];
        _audioSrc.Play();
    }

}