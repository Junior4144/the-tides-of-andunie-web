using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public List<InventoryItem> items;

    private Dictionary<string, InventoryItem> _lookup;

    public void Initialize()
    {
        _lookup = new Dictionary<string, InventoryItem>();

        foreach (var item in items)
        {
            if (!_lookup.ContainsKey(item.ItemId))
                _lookup.Add(item.ItemId, item);
            else
                Debug.LogWarning($"Duplicate ItemId detected: {item.ItemId}");
        }
    }

    public InventoryItem GetItem(string id)
    {
        if (_lookup == null)
            Initialize();

        return _lookup.TryGetValue(id, out var item) ? item : null;
    }
}
