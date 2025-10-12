using UnityEngine;
// Add the correct namespace for Item, for example:

public class PlayerItemCollector : MonoBehaviour
{
    private InventoryController inventoryController;
    private Collider2D itemCollider;

    private PlayerHealthController healthController;
    void Start()
    {
        inventoryController = FindFirstObjectByType<InventoryController>();
        itemCollider = GetComponent<Collider2D>();
        healthController = GetComponent<PlayerHealthController>();
    }

    private void OnTriggerEnter2D(Collider2D collison) 
    {

        if (collison.CompareTag("Item") && healthController != null)
        {
            Debug.Log($"{this.name} collided with {collison.name}");
            Item item = collison.GetComponent<Item>();
            if (item != null)
            {
                if (item.useImmediately)
                {
                    UseItem(item);
                    Destroy(collison.gameObject);
                }
                //Add item inventory
                else
                {
                    bool itemAdded = inventoryController.AddItem(collison.gameObject);
                    if (itemAdded)
                    {
                        Destroy(collison.gameObject);
                    }
                }
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
