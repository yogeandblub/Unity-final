using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private Transform slotsParent;      // RectTransform parent with GridLayoutGroup
    [SerializeField] private InventorySlotUI slotPrefab; // prefab with InventorySlotUI on root
    [SerializeField] private EquipManager equipManager;

    [Header("Spawn Settings")]
    [SerializeField] private Transform head;             // CenterEyeAnchor
    [SerializeField] private float spawnDistance = 0.9f;
    [SerializeField] private float spawnDownOffset = 0.1f;

    [Header("Behavior")]
    [SerializeField] private int fixedSlotCount = 8;     // 2x4
    [SerializeField] private bool tapSpawnsIfNotEquipable = true;
    [SerializeField] private bool rightHandEquip = true;

    private InventorySlotUI[] _slots;

    private void Awake()
    {
        BuildFixedSlots();
    }

    private void OnEnable()
    {
        if (inventory) inventory.OnInventoryChanged += RefreshUI;
    }

    private void OnDisable()
    {
        if (inventory) inventory.OnInventoryChanged -= RefreshUI;
    }

    private void Start()
    {
        RefreshUI();
    }

    private void BuildFixedSlots()
    {
        if (!slotsParent || !slotPrefab)
        {
            Debug.LogError("[InventoryUI] Missing slotsParent or slotPrefab.");
            return;
        }

        // Clear existing children (optional)
        for (int i = slotsParent.childCount - 1; i >= 0; i--)
            Destroy(slotsParent.GetChild(i).gameObject);

        _slots = new InventorySlotUI[fixedSlotCount];

        for (int i = 0; i < fixedSlotCount; i++)
        {
            var slot = Instantiate(slotPrefab, slotsParent);
            slot.name = $"Slot_{i}";
            slot.Bind(this, i);
            _slots[i] = slot;
        }
    }

    public void RefreshUI()
    {
        if (_slots == null || _slots.Length == 0) return;

        // Fill slots from inventory list order
        var items = inventory ? inventory.Items : null;

        for (int i = 0; i < _slots.Length; i++)
        {
            InventoryItem item = null;

            if (items != null && i < items.Count)
                item = items[i];

            _slots[i].SetItem(item);
        }

        Debug.Log($"[InventoryUI] RefreshUI count={(items != null ? items.Count : 0)}");
    }

    public void OnSlotTapped(int slotIndex)
    {
        var item = GetItemAtSlot(slotIndex);
        if (item == null || item.data == null) return;

        Debug.Log($"[InventoryUI] Tapped slot {slotIndex}: {item.data.displayName}");

        if (item.data.equipToHand && equipManager)
        {
            equipManager.Equip(item.data, rightHandEquip);
            return; // do NOT consume item on equip
        }

        if (tapSpawnsIfNotEquipable)
        {
            SpawnAndConsume(slotIndex, 1);
        }
    }

    public void OnSlotHeld(int slotIndex)
    {
        var item = GetItemAtSlot(slotIndex);
        if (item == null || item.data == null) return;

        Debug.Log($"[InventoryUI] Held slot {slotIndex}: {item.data.displayName}");
        SpawnAndConsume(slotIndex, 1);
    }

    private InventoryItem GetItemAtSlot(int slotIndex)
    {
        if (!inventory) return null;
        var items = inventory.Items;
        if (slotIndex < 0 || slotIndex >= items.Count) return null;
        return items[slotIndex];
    }

    private void SpawnAndConsume(int slotIndex, int amount)
    {
        var item = GetItemAtSlot(slotIndex);
        if (item == null || item.data == null) return;

        if (!item.data.worldPrefab)
        {
            Debug.LogError($"[InventoryUI] worldPrefab is NULL for: {item.data.displayName}");
            return;
        }

        if (!head)
        {
            Debug.LogError("[InventoryUI] Head transform not assigned (CenterEyeAnchor).");
            return;
        }

        Vector3 spawnPos = head.position + head.forward * spawnDistance + Vector3.down * spawnDownOffset;
        Quaternion spawnRot = Quaternion.identity;

        var go = Instantiate(item.data.worldPrefab, spawnPos, spawnRot);
        go.name = item.data.displayName + "_World";

        // Mark it as an item (for absorb later)
        var iw = go.GetComponent<ItemWorld>();
        if (!iw) iw = go.AddComponent<ItemWorld>();
        iw.Init(item.data, amount);

        // Physics toggle
        var rb = go.GetComponentInChildren<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = !item.data.spawnWithPhysics;
            rb.useGravity = item.data.spawnWithPhysics;
        }

        // Consume after spawn
        inventory.RemoveItem(item.data, amount);

        Debug.Log($"[InventoryUI] Spawned {go.name} at {spawnPos}");
    }
}
