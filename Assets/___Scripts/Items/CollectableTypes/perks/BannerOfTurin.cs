using UnityEngine;

public class BannerOfTurin : MonoBehaviour, IInventoryItem
{
    [SerializeField] string itemId;
    [SerializeField] string itemName;
    [SerializeField] bool stackable = true;
    [SerializeField] int maxStackSize = 20;

    public string ItemId => itemId;
    public string ItemName => itemName;
    public bool IsStackable => stackable;
    public int MaxStackSize => maxStackSize;

    [SerializeField]
    private GameObject inventoryIconPrefab;
    public GameObject InventoryIconPrefab => inventoryIconPrefab;
}
