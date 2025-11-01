using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Grub : MonoBehaviour, ICollectable
{
    [SerializeField] private int _amountOfHealth;
    [SerializeField] private AudioClip _pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Friendly")) return;
        HandlePickUp();
    }

    public void HandlePickUp()
    {
        if (PlayerManager.Instance.GetPercentHealth() > .98) return;

        PlayPickupSound();
        PlayerManager.Instance.AddHealth(_amountOfHealth);
        Destroy(gameObject);
    }

    private void PlayPickupSound()
    {
        if (_pickupSound != null)
            AudioSource.PlayClipAtPoint(_pickupSound, transform.position);
        else
            Debug.LogWarning("[Grub] PickupSound is null. Playing no sound");
    }
}
