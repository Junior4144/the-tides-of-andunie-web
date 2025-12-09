using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public abstract class Collectable : MonoBehaviour
{
    [SerializeField] protected AudioClip _pickupSound;

    protected AudioSource _audioSource;

    protected virtual void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    protected void PlayPickupSound()
    {
        if (_pickupSound != null)
        {
            _audioSource.PlayOneShot(_pickupSound);
            StartCoroutine(DestroyAfterSound());
        }
        else
        {
            Debug.LogWarning($"[{GetType().Name}] PickupSound is null. Playing no sound");
            Destroy(gameObject);
        }
    }

    protected IEnumerator DestroyAfterSound()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(_pickupSound.length);
        Destroy(gameObject);
    }
}
