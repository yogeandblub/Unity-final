using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public ItemDefinition item;
    public int amount;

    public bool IsEmpty => item == null || amount <= 0;

    public InventorySlot()
    {
        Clear();
    }

    public void Clear()
    {
        item = null;
        amount = 0;
    }
}

public class Inventory : MonoBehaviour
{
    [Header("Settings")]
    public int capacity = 12;

    [Header("Debug View")]
    public List<InventorySlot> slots = new List<InventorySlot>();

    public event Action OnInventoryChanged;

    private void Awake()
    {
        // Ensure slots list is correctly sized
        if (slots == null || slots.Count != capacity)
        {
            slots = new List<InventorySlot>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                slots.Add(new InventorySlot());
            }
        }
    }

    /// <summary>
    /// Try to add an item. Returns true if at least one was added.
    /// </summary>
    public bool AddItem(ItemDefinition item, int amount = 1)
    {
        if (item == null || amount <= 0)
            return false;

        int amountToAdd = amount;

        // 1. If stackable, try to fill existing stacks
        if (item.stackable)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
                if (!slot.IsEmpty && slot.item == item && slot.amount < item.maxStack)
                {
                    int canAdd = item.maxStack - slot.amount;
                    int addNow = Mathf.Min(canAdd, amountToAdd);
                    slot.amount += addNow;
                    amountToAdd -= addNow;

                    if (amountToAdd <= 0)
                    {
                        OnInventoryChanged?.Invoke();
                        return true;
                    }
                }
            }
        }

        // 2. Put remaining into empty slots
        for (int i = 0; i < slots.Count && amountToAdd > 0; i++)
        {
            var slot = slots[i];
            if (slot.IsEmpty)
            {
                int addNow = item.stackable
                    ? Mathf.Min(item.maxStack, amountToAdd)
                    : 1;

                slot.item = item;
                slot.amount = addNow;
                amountToAdd -= addNow;
            }
        }

        bool addedAnything = amountToAdd < amount;
        if (addedAnything)
        {
            OnInventoryChanged?.Invoke();
        }
        return addedAnything;
    }

    /// <summary>
    /// Try to remove an item. Returns true if fully removed requested amount.
    /// </summary>
    public bool RemoveItem(ItemDefinition item, int amount = 1)
    {
        if (item == null || amount <= 0)
            return false;

        int remaining = amount;

        // Go through slots and remove as needed
        for (int i = 0; i < slots.Count && remaining > 0; i++)
        {
            var slot = slots[i];
            if (!slot.IsEmpty && slot.item == item)
            {
                int removeNow = Mathf.Min(slot.amount, remaining);
                slot.amount -= removeNow;
                remaining -= removeNow;

                if (slot.amount <= 0)
                {
                    slot.Clear();
                }
            }
        }

        bool success = remaining <= 0;
        if (success)
        {
            OnInventoryChanged?.Invoke();
        }
        return success;
    }

    public bool HasItem(ItemDefinition item, int amount = 1)
    {
        if (item == null || amount <= 0)
            return false;

        int count = 0;
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty && slot.item == item)
            {
                count += slot.amount;
                if (count >= amount)
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Get current items (for UI, debugging, etc.)
    /// </summary>
    public IReadOnlyList<InventorySlot> GetSlots()
    {
        return slots;
    }
}
