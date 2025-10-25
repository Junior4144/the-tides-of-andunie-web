using UnityEngine;

public class InventoryItem : MonoBehaviour, IInventoryItem
{
    [SerializeField] private string itemId;
    [SerializeField] private string itemName;
    [SerializeField] private bool stackable;
    [SerializeField] private int maxStackSize;
    [SerializeField] private GameObject inventoryIconPrefab;
    [SerializeField] private ItemEffect[] _effects;

    public string ItemId => itemId;
    public string ItemName => itemName;
    public bool IsStackable => stackable;
    public int MaxStackSize => maxStackSize;
    public GameObject InventoryIconPrefab => inventoryIconPrefab;

    public ItemEffect[] GetEffects() => _effects;
}
