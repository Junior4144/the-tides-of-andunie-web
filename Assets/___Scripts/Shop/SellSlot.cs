using UnityEngine;
using UnityEngine.Audio;

public class SellSlot : MonoBehaviour
{
    public IInventoryItem currentItem;
    public AudioClip sellSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void SellItem()
    {
        if (currentItem == null) return;

        ShopManager.Instance.TryToSell(currentItem);

        if (sellSound != null)
            audioSource.PlayOneShot(sellSound);
    }
}
