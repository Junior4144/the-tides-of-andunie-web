using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SoundFxManager : MonoBehaviour
{
    public static SoundFxManager instance;

    [SerializeField] private AudioSource soundFXObject;


    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public void PlayerSoundFxClip(AudioClip audioClip, Transform spawnTransform, float volumeSpecifc)
    {
        if (soundFXObject == null) return;
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volumeSpecifc;
        
        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }
    public void PlayerRandomSoundFxClip(AudioClip[] audioClip, Transform spawnTransform, float volumeSpecifc)
    {
        int rand = Random.Range(0, audioClip.Length);


        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip[rand];

        audioSource.volume = volumeSpecifc;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }
}
