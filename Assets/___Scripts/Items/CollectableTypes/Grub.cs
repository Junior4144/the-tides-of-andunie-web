using UnityEngine;

public class Grub : MonoBehaviour, ICollectable
{
    [SerializeField]
    private int AmountOfHealth;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        HandlePickUp();
    }

    public void HandlePickUp() =>
        PlayerManager.Instance.AddHealth(AmountOfHealth);

}
