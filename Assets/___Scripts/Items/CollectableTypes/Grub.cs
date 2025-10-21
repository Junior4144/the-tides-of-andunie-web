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

    public void HandlePickUp()
    {
        if (PlayerManager.Instance.GetPercentHealth() > .98) return;

        PlayerManager.Instance.AddHealth(AmountOfHealth);
        Destroy(gameObject);
    }


}
