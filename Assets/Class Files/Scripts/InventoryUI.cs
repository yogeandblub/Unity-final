using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private Transform slotsParent;   // Grid/Vertical Layout
    [SerializeField] private GameObject slotPrefab;

    [Header("Spawn Settings")]
    public Transform head;       // CenterEyeAnchor
    public float spawnDistance = 0.7f;
    public float spawnHeightOffset = -0.1f;

    private readonly List<InventorySlotUI> _slots = new();

    private void OnEnable()
    {
        if (inventory != null)
            inventory.OnInventoryChanged += RefreshUI;
    }

    private void OnDisable()
    {
        if (inventory != null)
            inventory.OnInventoryChanged -= RefreshUI;
    }

    private void Start()
    {
        RefreshUI();
    }

    private void ClearSlots()
    {
        foreach (Transform child in slotsParent)
            Destroy(child.gameObject);

        _slots.Clear();
    }

    public void RefreshUI()
    {
        if (inventory == null) return;

        ClearSlots();

        foreach (var item in inventory.Items)
        {
            var go = Instantiate(slotPrefab, slotsParent);
            var slot = go.GetComponent<InventorySlotUI>();
            slot.Setup(item, this);
            _slots.Add(slot);
        }
    }

    public void SpawnItemFromSlot(InventoryItem item)
    {
        if (item == null || item.data == null || head == null)
            return;

        bool removed = inventory.RemoveItem(item.data, 1);
        if (!removed) return;

        // Spawn in front of head
        Vector3 forward = head.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 spawnPos = head.position
                         + forward * spawnDistance
                         + Vector3.up * spawnHeightOffset;

        Quaternion rot = Quaternion.LookRotation(forward, Vector3.up);

        if (item.data.worldPrefab != null)
        {
            var worldGO = Object.Instantiate(item.data.worldPrefab, spawnPos, rot);
            var iw = worldGO.GetComponent<ItemWorld>();
            if (iw != null)
                iw.Init(item.data, 1);
        }
    }
}
