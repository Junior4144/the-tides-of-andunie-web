using UnityEngine;

public class Grub : Collectable
{
    [SerializeField] private int _amountOfHealth;

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
    }
}
