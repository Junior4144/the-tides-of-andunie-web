using System.Diagnostics.Tracing;
using UnityEngine;

public class BlueBottleItem : MonoBehaviour, IInventoryItem
{
    public string ItemId => "BlueBottle";
    public string ItemName => "Blue Bottle";
    public bool IsStackable => true;
    public int MaxStackSize => 16;

    [SerializeField]
    private GameObject inventoryIconPrefab;
    public GameObject InventoryIconPrefab => inventoryIconPrefab;
}
