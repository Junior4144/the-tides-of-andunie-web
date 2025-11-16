using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    void Awake()
    {
        if (masterSlider != null)
        {
            masterSlider.value = 100;
            SetMasterVolume();
        }
        if (musicSlider != null)
        {
            musicSlider.value = 100;
            SetMusicVolume();
        }
        if (sfxSlider != null)
        {
            sfxSlider.value = 100;
            SetSFXVolume();
        }
    }

    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("Sfx", Mathf.Log10(volume) * 20);
    }
}
