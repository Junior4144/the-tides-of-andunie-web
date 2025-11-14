using UnityEngine;

public class InventoryItem : MonoBehaviour, IInventoryItem
{
    [SerializeField] private string itemId;
    [SerializeField] private string itemName;
    [SerializeField] private bool stackable;
    [SerializeField] private int maxStackSize;
    [SerializeField] private GameObject inventoryIconPrefab;
    [SerializeField] private ItemEffect[] _effects;
    [SerializeField] private Sprite spriteIcon;

    public string ItemId => itemId;
    public string ItemName => itemName;
    public bool IsStackable => stackable;
    public int MaxStackSize => maxStackSize;
    public GameObject InventoryIconPrefab => inventoryIconPrefab;
    public Sprite SpriteIcon => spriteIcon;
    public ItemEffect[] GetEffects() => _effects;
}
