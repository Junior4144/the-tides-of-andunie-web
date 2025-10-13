using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private InventoryController inventoryController;

    private PlayerHealthController healthController;
    void Start()
    {
        inventoryController = FindFirstObjectByType<InventoryController>();
        healthController = GetComponent<PlayerHealthController>();
    }

    private void OnTriggerEnter2D(Collider2D collison)
    {
        if (!collison.CompareTag("Item") || !healthController) return;

        if (collison.TryGetComponent<Item>(out var item))
        {
            if (item.useImmediately)
            {
                UseItem(item);
                Destroy(collison.gameObject);
            }
            else
            {
                if (inventoryController.AddItem(collison.gameObject))
                    Destroy(collison.gameObject);
            }
        }
    }

    private void UseItem(Item item)
    {
        string type = item.description[0];
        switch (type)
        {
            case "health":
                float amount = float.Parse(item.description[1]);
                if (amount > 0)
                    healthController.AddHealth(amount);
                else
                    healthController.TakeDamage(-amount);
                break;
        }
    }

}
