using UnityEngine;
// Add the correct namespace for Item, for example:

public class PlayerItemCollector : MonoBehaviour
{
    private InventoryController inventoryController;
    private Collider2D itemCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryController = FindFirstObjectByType<InventoryController>();
        itemCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collison) 
    {

        if (collison.CompareTag("Item") && GetComponent<PlayerHealthController>()) // IHealthController was removed -> causing error 
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
                {
                    GetComponent<PlayerHealthController>().AddHealth(amount);
                }
                else
                {
                    GetComponent<IHealthController>().TakeDamage(-amount);
                }
                break;
        }
    }

}
