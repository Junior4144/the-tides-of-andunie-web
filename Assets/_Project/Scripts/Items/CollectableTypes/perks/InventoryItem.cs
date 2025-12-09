using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private string itemId;
    [SerializeField] private string itemName;
    [SerializeField] private bool stackable;
    [SerializeField] private int maxStackSize;
    [SerializeField] private int sellAmount = 1;
    [SerializeField] private GameObject inventoryIconPrefab;
    [SerializeField] private Sprite spriteIcon;
    [SerializeField] private ItemEffect[] _effects;

    public string ItemId => itemId;
    public string ItemName => itemName;
    public bool IsStackable => stackable;
    public int MaxStackSize => maxStackSize;
    public int SellAmount => sellAmount;
    public GameObject InventoryIconPrefab => inventoryIconPrefab;
    public Sprite SpriteIcon => spriteIcon;
    public ItemEffect[] GetEffects() => _effects;
}
