using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RewardListing
{
    public MonoBehaviour inventoryItem; // must assign a prefab with IInventoryItem
    public int quantity;
    public IInventoryItem Item => (IInventoryItem)inventoryItem;
}