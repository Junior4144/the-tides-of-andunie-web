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
                //Add item inventory
                bool itemAdded = inventoryController.AddItem(collison.gameObject);
                if (itemAdded)
                {
                    Destroy(collison.gameObject);
                }
            }
        }
    }

}
