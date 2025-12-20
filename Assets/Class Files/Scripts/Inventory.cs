using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action OnInventoryChanged;

    [SerializeField] private List<InventoryItem> items = new();
    public IReadOnlyList<InventoryItem> Items => items;

    public void AddItem(ItemData data, int amount = 1)
    {
        if (data == null || amount <= 0) return;

        var existing = items.Find(i => i.data == data);
        if (existing != null) existing.amount += amount;
        else items.Add(new InventoryItem(data, amount));

        OnInventoryChanged?.Invoke();
    }

    public bool RemoveItem(ItemData data, int amount = 1)
    {
        if (data == null || amount <= 0) return false;

        var existing = items.Find(i => i.data == data);
        if (existing == null) return false;

        existing.amount -= amount;
        if (existing.amount <= 0) items.Remove(existing);

        OnInventoryChanged?.Invoke();
        return true;
    }

    public int GetCount(ItemData data)
    {
        var existing = items.Find(i => i.data == data);
        return existing?.amount ?? 0;
    }
}
