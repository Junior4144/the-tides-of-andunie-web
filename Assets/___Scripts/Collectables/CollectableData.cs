using UnityEngine;

[CreateAssetMenu(menuName = "Collectables/Data")]
public class CollectableData : ScriptableObject, IInventoryItem
{
    public CollectableType type;
    public GameObject inventoryIconPrefab;
    public float amount;
    public bool destroyOnPickup = true;

    [Header("Inventory Item Data")]
    [SerializeField] private string itemId;
    [SerializeField] private string itemName;
    [SerializeField] private bool stackable = true;
    [SerializeField] private int maxStackSize = 20;

    // IInventoryItem Implementation
    public string ItemId => itemId;
    public string ItemName => itemName;
    public bool IsStackable => stackable;
    public int MaxStackSize => maxStackSize;
}