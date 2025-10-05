using UnityEngine;
// Add the correct namespace for Item, for example:

public class PlayerItemCollector : MonoBehaviour
{
    private InventoryController inventoryController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryController = FindFirstObjectByType<InventoryController>();

    }

    private void OnTriggerEnter2D(Collider2D collison) {

        if (collison.CompareTag("Item"))
        {
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
                {
                    transform.parent.GetComponent<HealthController>().AddHealth(amount);
                }
                else
                {
                    transform.parent.GetComponent<HealthController>().TakeDamage(-amount);
                }
                break;
        }
    }

}
