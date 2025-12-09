using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RegionClickPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip clickClip;
    [SerializeField] private float volumeScale = 1f;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        OnClickOutline.RegionClicked += HandleRegionClicked;
    }

    private void OnDisable()
    {
        OnClickOutline.RegionClicked -= HandleRegionClicked;
    }

    private void HandleRegionClicked(Region region)
    {
        PlayOneShot(clickClip, volumeScale);
    }

    public void PlayOneShot(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip, volumeScale);
        else
            Debug.LogWarning("[RegionClickPlayer] AudioClip is null. Skipping sound.");
    }
}
